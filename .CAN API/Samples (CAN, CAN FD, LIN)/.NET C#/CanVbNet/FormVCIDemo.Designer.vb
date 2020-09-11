<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormVCIV4Demo
  Inherits System.Windows.Forms.Form

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    If disposing AndAlso components IsNot Nothing Then
      components.Dispose()
    End If
    MyBase.Dispose(disposing)
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ButtonClose = New System.Windows.Forms.Button()
        Me.ButtonInit = New System.Windows.Forms.Button()
        Me.LabelLastRxMsgCaption = New System.Windows.Forms.Label()
        Me.labelLastRxMsg = New System.Windows.Forms.Label()
        Me.ButtonTransmitData = New System.Windows.Forms.Button()
        Me.PictureBoxInitMode = New System.Windows.Forms.PictureBox()
        Me.PictureBoxTxPending = New System.Windows.Forms.PictureBox()
        Me.PictureBoxOverrun = New System.Windows.Forms.PictureBox()
        Me.PictureBoxWarningLevel = New System.Windows.Forms.PictureBox()
        Me.PictureBoxBusOff = New System.Windows.Forms.PictureBox()
        Me.LabelInitMode = New System.Windows.Forms.Label()
        Me.LabelTxPending = New System.Windows.Forms.Label()
        Me.LabelOverrun = New System.Windows.Forms.Label()
        Me.LabelWarningLevel = New System.Windows.Forms.Label()
        Me.LabelBusOff = New System.Windows.Forms.Label()
        Me.TimerGetStatus = New System.Windows.Forms.Timer(Me.components)
        Me.LabelCaptionListBox = New System.Windows.Forms.Label()
        Me.ListViewAvailInterfaces = New System.Windows.Forms.ListView()
        Me.TimerSendData = New System.Windows.Forms.Timer(Me.components)
        Me.LabelHardwareID = New System.Windows.Forms.Label()
        Me.LblHWID = New System.Windows.Forms.Label()
        Me.btnAcceptanceAll = New System.Windows.Forms.Button()
        Me.btnAcceptanceID1 = New System.Windows.Forms.Button()
        Me.ListBoxBaudrate = New System.Windows.Forms.ListBox()
        Me.LabelBaudListBox = New System.Windows.Forms.Label()
        Me.LabelFilterDescription = New System.Windows.Forms.Label()
        CType(Me.PictureBoxInitMode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBoxTxPending, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBoxOverrun, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBoxWarningLevel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBoxBusOff, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ButtonClose
        '
        Me.ButtonClose.Location = New System.Drawing.Point(390, 259)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(75, 23)
        Me.ButtonClose.TabIndex = 0
        Me.ButtonClose.Text = "Close"
        Me.ButtonClose.UseVisualStyleBackColor = True
        '
        'ButtonInit
        '
        Me.ButtonInit.Enabled = False
        Me.ButtonInit.Location = New System.Drawing.Point(204, 15)
        Me.ButtonInit.Name = "ButtonInit"
        Me.ButtonInit.Size = New System.Drawing.Size(75, 23)
        Me.ButtonInit.TabIndex = 1
        Me.ButtonInit.Text = "Initialize"
        Me.ButtonInit.UseVisualStyleBackColor = True
        '
        'LabelLastRxMsgCaption
        '
        Me.LabelLastRxMsgCaption.AutoSize = True
        Me.LabelLastRxMsgCaption.Location = New System.Drawing.Point(20, 181)
        Me.LabelLastRxMsgCaption.Name = "LabelLastRxMsgCaption"
        Me.LabelLastRxMsgCaption.Size = New System.Drawing.Size(125, 13)
        Me.LabelLastRxMsgCaption.TabIndex = 2
        Me.LabelLastRxMsgCaption.Text = "Last Received Message:"
        '
        'labelLastRxMsg
        '
        Me.labelLastRxMsg.BackColor = System.Drawing.SystemColors.Window
        Me.labelLastRxMsg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.labelLastRxMsg.Location = New System.Drawing.Point(18, 204)
        Me.labelLastRxMsg.Name = "labelLastRxMsg"
        Me.labelLastRxMsg.Size = New System.Drawing.Size(401, 23)
        Me.labelLastRxMsg.TabIndex = 3
        '
        'ButtonTransmitData
        '
        Me.ButtonTransmitData.Enabled = False
        Me.ButtonTransmitData.Location = New System.Drawing.Point(216, 259)
        Me.ButtonTransmitData.Name = "ButtonTransmitData"
        Me.ButtonTransmitData.Size = New System.Drawing.Size(135, 23)
        Me.ButtonTransmitData.TabIndex = 4
        Me.ButtonTransmitData.Text = "Transmit Message"
        Me.ButtonTransmitData.UseVisualStyleBackColor = True
        '
        'PictureBoxInitMode
        '
        Me.PictureBoxInitMode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBoxInitMode.Location = New System.Drawing.Point(353, 15)
        Me.PictureBoxInitMode.Name = "PictureBoxInitMode"
        Me.PictureBoxInitMode.Size = New System.Drawing.Size(12, 12)
        Me.PictureBoxInitMode.TabIndex = 5
        Me.PictureBoxInitMode.TabStop = False
        '
        'PictureBoxTxPending
        '
        Me.PictureBoxTxPending.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBoxTxPending.Location = New System.Drawing.Point(353, 33)
        Me.PictureBoxTxPending.Name = "PictureBoxTxPending"
        Me.PictureBoxTxPending.Size = New System.Drawing.Size(12, 12)
        Me.PictureBoxTxPending.TabIndex = 6
        Me.PictureBoxTxPending.TabStop = False
        '
        'PictureBoxOverrun
        '
        Me.PictureBoxOverrun.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBoxOverrun.Location = New System.Drawing.Point(353, 51)
        Me.PictureBoxOverrun.Name = "PictureBoxOverrun"
        Me.PictureBoxOverrun.Size = New System.Drawing.Size(12, 12)
        Me.PictureBoxOverrun.TabIndex = 7
        Me.PictureBoxOverrun.TabStop = False
        '
        'PictureBoxWarningLevel
        '
        Me.PictureBoxWarningLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBoxWarningLevel.Location = New System.Drawing.Point(353, 70)
        Me.PictureBoxWarningLevel.Name = "PictureBoxWarningLevel"
        Me.PictureBoxWarningLevel.Size = New System.Drawing.Size(12, 12)
        Me.PictureBoxWarningLevel.TabIndex = 8
        Me.PictureBoxWarningLevel.TabStop = False
        '
        'PictureBoxBusOff
        '
        Me.PictureBoxBusOff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBoxBusOff.Location = New System.Drawing.Point(353, 88)
        Me.PictureBoxBusOff.Name = "PictureBoxBusOff"
        Me.PictureBoxBusOff.Size = New System.Drawing.Size(12, 12)
        Me.PictureBoxBusOff.TabIndex = 9
        Me.PictureBoxBusOff.TabStop = False
        '
        'LabelInitMode
        '
        Me.LabelInitMode.AutoSize = True
        Me.LabelInitMode.Location = New System.Drawing.Point(371, 15)
        Me.LabelInitMode.Name = "LabelInitMode"
        Me.LabelInitMode.Size = New System.Drawing.Size(50, 13)
        Me.LabelInitMode.TabIndex = 10
        Me.LabelInitMode.Text = "Init mode"
        '
        'LabelTxPending
        '
        Me.LabelTxPending.AutoSize = True
        Me.LabelTxPending.Location = New System.Drawing.Point(371, 32)
        Me.LabelTxPending.Name = "LabelTxPending"
        Me.LabelTxPending.Size = New System.Drawing.Size(60, 13)
        Me.LabelTxPending.TabIndex = 11
        Me.LabelTxPending.Text = "Tx pending"
        '
        'LabelOverrun
        '
        Me.LabelOverrun.AutoSize = True
        Me.LabelOverrun.Location = New System.Drawing.Point(371, 51)
        Me.LabelOverrun.Name = "LabelOverrun"
        Me.LabelOverrun.Size = New System.Drawing.Size(71, 13)
        Me.LabelOverrun.TabIndex = 12
        Me.LabelOverrun.Text = "Data Overrun"
        '
        'LabelWarningLevel
        '
        Me.LabelWarningLevel.AutoSize = True
        Me.LabelWarningLevel.Location = New System.Drawing.Point(371, 70)
        Me.LabelWarningLevel.Name = "LabelWarningLevel"
        Me.LabelWarningLevel.Size = New System.Drawing.Size(94, 13)
        Me.LabelWarningLevel.TabIndex = 13
        Me.LabelWarningLevel.Text = "Error warning level"
        '
        'LabelBusOff
        '
        Me.LabelBusOff.AutoSize = True
        Me.LabelBusOff.Location = New System.Drawing.Point(371, 88)
        Me.LabelBusOff.Name = "LabelBusOff"
        Me.LabelBusOff.Size = New System.Drawing.Size(40, 13)
        Me.LabelBusOff.TabIndex = 14
        Me.LabelBusOff.Text = "Bus off"
        '
        'TimerGetStatus
        '
        '
        'LabelCaptionListBox
        '
        Me.LabelCaptionListBox.AutoSize = True
        Me.LabelCaptionListBox.Location = New System.Drawing.Point(20, 20)
        Me.LabelCaptionListBox.Name = "LabelCaptionListBox"
        Me.LabelCaptionListBox.Size = New System.Drawing.Size(134, 13)
        Me.LabelCaptionListBox.TabIndex = 16
        Me.LabelCaptionListBox.Text = "Available IXXAT Interfaces"
        '
        'ListViewAvailInterfaces
        '
        Me.ListViewAvailInterfaces.GridLines = True
        Me.ListViewAvailInterfaces.HideSelection = False
        Me.ListViewAvailInterfaces.Location = New System.Drawing.Point(18, 44)
        Me.ListViewAvailInterfaces.Name = "ListViewAvailInterfaces"
        Me.ListViewAvailInterfaces.Size = New System.Drawing.Size(180, 92)
        Me.ListViewAvailInterfaces.TabIndex = 17
        Me.ListViewAvailInterfaces.UseCompatibleStateImageBehavior = False
        Me.ListViewAvailInterfaces.View = System.Windows.Forms.View.List
        '
        'TimerSendData
        '
        Me.TimerSendData.Interval = 1
        '
        'LabelHardwareID
        '
        Me.LabelHardwareID.AutoSize = True
        Me.LabelHardwareID.Location = New System.Drawing.Point(20, 139)
        Me.LabelHardwareID.Name = "LabelHardwareID"
        Me.LabelHardwareID.Size = New System.Drawing.Size(147, 13)
        Me.LabelHardwareID.TabIndex = 18
        Me.LabelHardwareID.Text = "Hardware ID / Serial Number:"
        '
        'LblHWID
        '
        Me.LblHWID.AutoSize = True
        Me.LblHWID.BackColor = System.Drawing.SystemColors.Window
        Me.LblHWID.Location = New System.Drawing.Point(173, 139)
        Me.LblHWID.Name = "LblHWID"
        Me.LblHWID.Size = New System.Drawing.Size(10, 13)
        Me.LblHWID.TabIndex = 19
        Me.LblHWID.Text = "-"
        '
        'btnAcceptanceAll
        '
        Me.btnAcceptanceAll.Location = New System.Drawing.Point(18, 259)
        Me.btnAcceptanceAll.Name = "btnAcceptanceAll"
        Me.btnAcceptanceAll.Size = New System.Drawing.Size(75, 23)
        Me.btnAcceptanceAll.TabIndex = 20
        Me.btnAcceptanceAll.Text = "Filter off"
        Me.btnAcceptanceAll.UseVisualStyleBackColor = True
        '
        'btnAcceptanceID1
        '
        Me.btnAcceptanceID1.Location = New System.Drawing.Point(99, 259)
        Me.btnAcceptanceID1.Name = "btnAcceptanceID1"
        Me.btnAcceptanceID1.Size = New System.Drawing.Size(75, 23)
        Me.btnAcceptanceID1.TabIndex = 21
        Me.btnAcceptanceID1.Text = "Filter on"
        Me.btnAcceptanceID1.UseVisualStyleBackColor = True
        '
        'ListBoxBaudrate
        '
        Me.ListBoxBaudrate.FormattingEnabled = True
        Me.ListBoxBaudrate.Items.AddRange(New Object() {"125", "250", "500", "800", "1000"})
        Me.ListBoxBaudrate.Location = New System.Drawing.Point(204, 67)
        Me.ListBoxBaudrate.Name = "ListBoxBaudrate"
        Me.ListBoxBaudrate.Size = New System.Drawing.Size(66, 69)
        Me.ListBoxBaudrate.TabIndex = 22
        '
        'LabelBaudListBox
        '
        Me.LabelBaudListBox.AutoSize = True
        Me.LabelBaudListBox.Location = New System.Drawing.Point(204, 44)
        Me.LabelBaudListBox.Name = "LabelBaudListBox"
        Me.LabelBaudListBox.Size = New System.Drawing.Size(35, 13)
        Me.LabelBaudListBox.TabIndex = 23
        Me.LabelBaudListBox.Text = "kBit/s"
        '
        'LabelFilterDescription
        '
        Me.LabelFilterDescription.AutoSize = True
        Me.LabelFilterDescription.Location = New System.Drawing.Point(15, 243)
        Me.LabelFilterDescription.Name = "LabelFilterDescription"
        Me.LabelFilterDescription.Size = New System.Drawing.Size(230, 13)
        Me.LabelFilterDescription.TabIndex = 24
        Me.LabelFilterDescription.Text = "Set/Reset a filter for all identifier with Bit 0 is set"
        '
        'FormVCIV4Demo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(472, 294)
        Me.Controls.Add(Me.LabelFilterDescription)
        Me.Controls.Add(Me.LabelBaudListBox)
        Me.Controls.Add(Me.ListBoxBaudrate)
        Me.Controls.Add(Me.btnAcceptanceID1)
        Me.Controls.Add(Me.btnAcceptanceAll)
        Me.Controls.Add(Me.LblHWID)
        Me.Controls.Add(Me.LabelHardwareID)
        Me.Controls.Add(Me.ListViewAvailInterfaces)
        Me.Controls.Add(Me.LabelCaptionListBox)
        Me.Controls.Add(Me.LabelBusOff)
        Me.Controls.Add(Me.LabelWarningLevel)
        Me.Controls.Add(Me.LabelOverrun)
        Me.Controls.Add(Me.LabelTxPending)
        Me.Controls.Add(Me.LabelInitMode)
        Me.Controls.Add(Me.PictureBoxBusOff)
        Me.Controls.Add(Me.PictureBoxWarningLevel)
        Me.Controls.Add(Me.PictureBoxOverrun)
        Me.Controls.Add(Me.PictureBoxTxPending)
        Me.Controls.Add(Me.PictureBoxInitMode)
        Me.Controls.Add(Me.ButtonTransmitData)
        Me.Controls.Add(Me.labelLastRxMsg)
        Me.Controls.Add(Me.LabelLastRxMsgCaption)
        Me.Controls.Add(Me.ButtonInit)
        Me.Controls.Add(Me.ButtonClose)
        Me.Name = "FormVCIV4Demo"
        Me.Text = "VCI4 VB .NET Example"
        CType(Me.PictureBoxInitMode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBoxTxPending, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBoxOverrun, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBoxWarningLevel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBoxBusOff, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
  Friend WithEvents ButtonClose As System.Windows.Forms.Button
  Friend WithEvents ButtonInit As System.Windows.Forms.Button
  Friend WithEvents LabelLastRxMsgCaption As System.Windows.Forms.Label
  Friend WithEvents labelLastRxMsg As System.Windows.Forms.Label
  Friend WithEvents ButtonTransmitData As System.Windows.Forms.Button
  Friend WithEvents PictureBoxInitMode As System.Windows.Forms.PictureBox
  Friend WithEvents PictureBoxTxPending As System.Windows.Forms.PictureBox
  Friend WithEvents PictureBoxOverrun As System.Windows.Forms.PictureBox
  Friend WithEvents PictureBoxWarningLevel As System.Windows.Forms.PictureBox
  Friend WithEvents PictureBoxBusOff As System.Windows.Forms.PictureBox
  Friend WithEvents LabelInitMode As System.Windows.Forms.Label
  Friend WithEvents LabelTxPending As System.Windows.Forms.Label
  Friend WithEvents LabelOverrun As System.Windows.Forms.Label
  Friend WithEvents LabelWarningLevel As System.Windows.Forms.Label
  Friend WithEvents LabelBusOff As System.Windows.Forms.Label
  Friend WithEvents TimerGetStatus As System.Windows.Forms.Timer
  Friend WithEvents LabelCaptionListBox As System.Windows.Forms.Label
  Friend WithEvents ListViewAvailInterfaces As System.Windows.Forms.ListView
  Friend WithEvents TimerSendData As System.Windows.Forms.Timer
  Friend WithEvents LabelHardwareID As System.Windows.Forms.Label
  Friend WithEvents LblHWID As System.Windows.Forms.Label
  Friend WithEvents btnAcceptanceAll As System.Windows.Forms.Button
  Friend WithEvents btnAcceptanceID1 As System.Windows.Forms.Button
  Friend WithEvents ListBoxBaudrate As System.Windows.Forms.ListBox
  Friend WithEvents LabelBaudListBox As System.Windows.Forms.Label
  Friend WithEvents LabelFilterDescription As System.Windows.Forms.Label

End Class
