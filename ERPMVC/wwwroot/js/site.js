

// Write your Javascript code.
var options = {
    element: '#viewer', // id of element that will contain  the viewer
    uiType: 'desktop',
    //reportService: { url: 'http://localhost:62533/ActiveReports.ReportService.asmx' },
    reportService: { url: 'http://localhost:62533/CustomReportService.asmx' },
    uiType: 'desktop',
    reportLoaded: function () {
        reportsButtons.prop('disabled', false);
    }

};
