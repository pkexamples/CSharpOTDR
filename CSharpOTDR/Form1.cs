using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpOTDR
{
	public partial class Form1 : Form
	{
		private GNOTDRSIGNATURELib.GNOTDRSignature sig = null;
		private GNOTDRSIGNATURESERVERLib.GNOTDRSignatureServer svr = null;
		private GNOTDRSIGCONNECTIONMGRLib.GNOTDRSigConnectionMgr conn = null;
		private GNOTDRBELLCOREHANDLERLib.GNOTDRBellcoreHandler bc = null;
		private GN8000Lib.GN8000 gn8k = null;
		private GNOTDRLib.GNOTDR otdr = null;
		private GNGRAPHLib.GNMarker actMkr;
		private GNGRAPHLib.GNMarker refMkr;
		private short sigHandle = -1;
		private bool graphNeedsSetup = false;
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
			Enabled = false;
			Task.Factory.StartNew(Startup, CancellationToken.None, TaskCreationOptions.HideScheduler).
				ContinueWith(StartupContinuation, CancellationToken.None,
					TaskContinuationOptions.HideScheduler, TaskScheduler.FromCurrentSynchronizationContext());
		}
		private void Startup(object state)
		{
			// This runs on a background thread from StartNew so it doesn't tie up the UI. Do not touch UI controls here!
			bc = new GNOTDRBELLCOREHANDLERLib.GNOTDRBellcoreHandler();
			svr = new GNOTDRSIGNATURESERVERLib.GNOTDRSignatureServer();
			sigHandle = svr.Add();
			sig = (GNOTDRSIGNATURELib.GNOTDRSignature)svr.get_Signature(sigHandle);
			sig.OnDataChange += sig_OnDataChange;
			conn = new GNOTDRSIGCONNECTIONMGRLib.GNOTDRSigConnectionMgr();
			conn.ConnectServer(svr);
			conn.MakeConnection(sigHandle, axGNGraph1.GetOcx(), svr.CreateRenderer(), "conn1");
			GN8000Lib.OTDREnumerator otdrEnum = new GN8000Lib.OTDREnumerator();
			for (int i = 0; i < otdrEnum.NumberOfFunctions; i++)
			{
				if (otdrEnum.get_IsOTDR(i))
				{
					gn8k = new GN8000Lib.GN8000();
					gn8k.Initialize((short)i, svr);
					if (gn8k.ONLINE != 0)
					{
						otdr = (GNOTDRLib.GNOTDR)gn8k.OTDR;
						otdr.OTDRAcquisitionProgress += otdr_OTDRAcquisitionProgress;
					}
					else
					{
						gn8k = null;
					}
					break;
				}
			}
		}
		private void StartupContinuation(Task t)
		{
			// Now Startup is done and we're back on the UI thread so you can mess with controls.
			if (t.Status == TaskStatus.RanToCompletion)
			{
				// everythang's groovy
				Cursor = Cursors.Default;
				Enabled = true;
				chkAverage.Enabled = otdr != null;
			}
			else
			{
				// (sad face)
				MessageBox.Show(String.Format("Problem! {0}", t.Exception.InnerException.Message), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				Close();
			}
			actMkr = (GNGRAPHLib.GNMarker)axGNGraph1.Markers.Add();
			refMkr = (GNGRAPHLib.GNMarker)axGNGraph1.Markers.Add();
			actMkr.MarkerMoved += mkr_MarkerMoved;
			refMkr.MarkerMoved += mkr_MarkerMoved;
			refMkr.Style = GNGRAPHLib.LineStyle.lsDash;
		}

		private void SetupGraph()
		{
			// Max log is never more than 32; min can depend
			short minY = sig.Trace.MinLogData;
			axGNGraph1.SetLogMinMax(0.0, sig.Range, minY, 32000.0);
			axGNGraph1.SetLogView(0.0, sig.Range, minY, 32000.0);
			axGNGraph1.Grid().HorizDivisions = 8;
			axGNGraph1.Grid().VertDivisions = 6;
			actMkr.LogXPosition = sig.Range / 3;
			refMkr.LogXPosition = 2 * sig.Range / 3;
		}

		private void cmdOpenFile_Click(object sender, EventArgs e)
		{
			// dispense with dialog for now and hard code path
			using (OpenFileDialog ofd = new OpenFileDialog())
			{
				ofd.Filter = "Bellcore OTDR files (*.sor)|*.sor|All files|*.*";
				if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					bc.Read(ofd.FileName, sig);
					SetupGraph();
				}
			}
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (sigHandle >= 0)
			{
				if (conn != null)
				{
					conn.BreakAllWithSigID(sigHandle);
				}
				System.Runtime.InteropServices.Marshal.FinalReleaseComObject(sig);
				svr.Remove(sigHandle);
				sigHandle = -1;
				System.Runtime.InteropServices.Marshal.FinalReleaseComObject(svr);
				svr = null;
			}
			if (gn8k != null)
			{
				System.Runtime.InteropServices.Marshal.FinalReleaseComObject(gn8k);
				gn8k = null;
			}
			if (conn != null)
			{
				conn.BreakAll();
				System.Runtime.InteropServices.Marshal.FinalReleaseComObject(conn);
			}
		}

		void otdr_OTDRAcquisitionStart()
		{
			otdr.OTDRAcquisitionStart -= otdr_OTDRAcquisitionStart;
			otdr.OTDRAcquisitionStop += otdr_OTDRAcquisitionStop;
		}

		void otdr_OTDRAcquisitionStop(GNOTDRLib.GNOTDRStopReason status)
		{
			// You'll want to check if status is an error or if this is an expected stop
			BeginInvoke((Action)(() =>
				{
					// Any controls that need to be disabled during acquisition can be placed inside pnlControls
					pnlControls.Enabled = true;
					chkAverage.Tag = new object();
					chkAverage.Checked = false;
					chkAverage.Tag = null;
					chkAverage.Text = "Average";
				}));
			otdr.OTDRAcquisitionStop -= otdr_OTDRAcquisitionStop;
		}

		void otdr_OTDRAcquisitionProgress(int progress)
		{
			// Run a progress bar if you like. Don't forget to BeginInvoke it.
		}

		void sig_OnDataChange(short type)
		{
			// Set up graph on first data update after acquisition starts
			if (graphNeedsSetup && type == (short)GNOTDRSIGNATURELib.OTDRTraceDataType.LogData)
			{
				Invoke((Action)(() => SetupGraph()));
				graphNeedsSetup = false;
			}
		}

		void mkr_MarkerMoved(short nId, double newX, double newY, int dx, int dy)
		{
			if (nId == 0)
			{
				// act mkr moved
			}
			else
			{
				// ref mkr moved
			}
		}

		private void chkAverage_CheckedChanged(object sender, EventArgs e)
		{
			// No action if tag set
			if (chkAverage.Tag != null) return;

			if (chkAverage.Checked)
			{
				// Set up your acquisition conditions. Hard coded here for simplicity
				chkAverage.Text = "Stop";
				double ONE_KM = 1e-5;	// Approx 1 km round trip in s
				otdr.Range = 32.0 * ONE_KM;
				otdr.WavelengthIndex = 0;
				otdr.Pulsewidth = 0.02 * ONE_KM;
				otdr.Pointspacing = .002 * ONE_KM;
				otdr.Port = 0;
				otdr.AcquisitionMode = GNOTDRLib.GNOTDRAcquisitionMode.AvgTime;
				otdr.AverageTime = 5.0;
				graphNeedsSetup = true;
				otdr.OTDRAcquisitionStart += otdr_OTDRAcquisitionStart;
				otdr.Acquire(sigHandle);
				pnlControls.Enabled = false;
			}
			else
			{
				otdr.Stop();
			}
		}
	}
}
