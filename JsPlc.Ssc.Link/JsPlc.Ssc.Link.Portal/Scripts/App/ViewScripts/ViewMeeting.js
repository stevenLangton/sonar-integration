require(["jquery", "knockout", "moment", "bootstrap-datepicker", "bootstrap-datepickerGB", "URI", "underscore", "common", "helpers", "toastr", "autogrow"],
    function ($, ko, moment, datepicker, datePickerGb, URI, _, common, helpers, toastr, autogrow) {

    function PageViewModel() {

        var self = this;

        self.dataModel = ko.observable(""); // LinkForm object
        self.dataAvailable = ko.observable(false);

        self.bind = function () { };

        var meetingView;
        var buildViewModels = function (data) {
            moment.locale("en-gb"); // Set Locale for moment (aka moment.locale("en-gb"))
            // Refer to Model values http://localhost/JsPlc.Ssc.Link.Service/api/Meetings/?employeeId=E001
            //var meetingDate = moment(data.MeetingDate).format("L"); // we get dd/mm/yyyy
            debugger;
            meetingView = {
                EmployeeId: data.EmployeeId,
                ColleagueId: data.ColleagueId,
                ManagerId: data.ManagerId,
                PeriodId: data.PeriodId,
                ManagerName: data.ManagerName,
                ColleagueName: data.ColleagueName,
                Colleague: data.Colleague,
                Manager: data.Manager,
                MeetingDate: data.MeetingDate,
                ColleagueSignedOffDate: data.ColleagueSignedOffDate,
                ManagerSignedOffDate: data.ManagerSignedOffDate,
                MeetingId: data.MeetingId,
                IsColleagueView: data.ColleagueInitiated,
                //Completed: data.Status,
                SharingDate: data.SharingDate,
                SharingStatus: data.SharingStatus,
                ColleagueSignOff: data.ColleagueSignOff,
                ManagerSignOff: data.ManagerSignOff,
                LookingBackQuestions: [],
                LookingFwdQuestions: [],
                DrivingDevelopmentQuestions: [],
                InANutShellQuestions: [],
                Questions: [],
                DownloadUrl: common.getSiteRoot() + 'pdf/DownloadFromDb/?MeetingId=' + data.MeetingId
            };
            $("input[name='CheckboxShared']").prop('checked', (data.SharingStatus == 1) ? true : false);
            $("input[name='CompletedMgr']").prop('checked', (data.ManagerSignOff == 1) ? true : false);

            ko.utils.arrayForEach(data.Questions, function (ques) {
                if (!ques.ColleagueComment)
                { ques.ColleagueComment = ""; } // NOTE TextArea and other input elements need to be bound to "value" not just text, otherwise we dont see user changes in model
                if (!ques.ManagerComment)
                { ques.ManagerComment = ""; }
            });
            meetingView.LookingBackQuestions = _.select(data.Questions, function (ques) { return ques.QuestionType == 'LOOKING BACK'; });
            meetingView.LookingFwdQuestions = _.select(data.Questions, function (ques) { return ques.QuestionType == 'LOOKING FORWARD'; });
            meetingView.DrivingDevelopmentQuestions = _.select(data.Questions, function (ques) { return ques.QuestionType == 'DRIVING MY DEVELOPMENT'; });
            meetingView.InANutShellQuestions = _.select(data.Questions, function (ques) { return ques.QuestionType == 'IN A NUTSHELL'; });

            self.dataModel(meetingView);
        };
        self.utils = helpers.utils;

        self.formatDateMonthDYHM = function (dateObj) {
            if (!dateObj) return '-';
            var formattedString = moment(dateObj, "").format('dddd, MMMM Do YYYY [at] HH:mma');
            return formattedString;
        }

        // ### GET LinkForm Data (assume there is data, it will show up), we may have to build a Get method which returns a blank Link Meeting template
        self.loadPageData = function (meetingId) {
            $.ajax({
                url: common.getSiteRoot() + "LinkForm/GetMeetingView/?meetingId=" + meetingId,
                method: "GET"
            })
                .done(function (data) {
                    if (data == "Error") {
                        $('#msgs').html("Error - no data");
                        self.dataAvailable(false);
                    }
                    else {
                        buildViewModels(data);
                        self.dataAvailable(true);
                    }
                    self.bind();
                })
                .fail(function () {
                    self.dataAvailable(false);
                    $('#msgs').html("Error occured");
                    self.bind();
                    // if cannot load LinkMeeting for the given period
                });
        }
        var doMeetingAction = function(actionUnshareOrApprove) {

            var meetingVm = self.dataModel();
            var postBackUrl = actionUnshareOrApprove == "unshare" ? "LinkForm/Unshare/" : "LinkForm/Approve/";
            var viewUrl = common.getSiteRoot() + "LinkForm/ViewMeeting/" + meetingVm.MeetingId;
            var editUrl = common.getSiteRoot() + "LinkForm/Edit/" + meetingVm.MeetingId;
            var teamUrl = common.getSiteRoot() + "Team";

            // ajax server side method to unshare meeting.
            $.ajax({
                    url: common.getSiteRoot() + postBackUrl + "?meetingId=" + meetingVm.MeetingId,
                    method: "GET",
                    dataType: "json"
                })
                .done(function(data) {
                    // If approved or unshared in meanwhile, Reload page with toastr notification..
                    if (actionUnshareOrApprove == "unshare" && data == "Approved") {
                        toastr.error("Sorry, you cannot amend this conversation. Conversation has been approved by line manager.");
                        window.setTimeout('window.location.href = "' + viewUrl + '";', 1000);
                    } else if (actionUnshareOrApprove == "approve" && data == "Unshared") {
                        toastr.error("Sorry, you cannot amend this conversation. Conversation has not been shared.");
                        window.setTimeout('window.location.href = "' + teamUrl + '";', 1000);
                    } else if (data == "Error") {
                        toastr.error("Sorry, unable to amend this conversation.");
                        window.setTimeout('window.location.href = "' + viewUrl + '";', 1000);
                    } else {
                        window.setTimeout('window.location.href = "' + editUrl + '";', 100);
                    }
                })
                .fail(function() {
                    toastr.error("Sorry, unable to amend this conversation. Please try again.");
                    window.setTimeout('window.location.href = "' + viewUrl + '";', 1000);
                });
        }
        self.unshareMeeting = function () {
            doMeetingAction('unshare');
        }
        self.approveMeeting = function () {
            doMeetingAction('approve');
        }

    }

    $(document).ready(function () {
        moment.locale("en-gb"); // Set Locale for moment (aka moment.locale("en-gb"))
        // knockout locale based date formatting - ko.observable(dateFormat(date, "dd/mm/yyyy"));
        // bootstrap datepicker formatting =  $("#meetingDatePicker").datepicker({dateFormat: 'dd/mm/yy'});

        var vm = new PageViewModel();

        var binder = function () {
            ko.applyBindings(vm, $("#linkpage")[0]); // important - we have to refer to div element with index 0.
            $('textarea').autogrow({ onInitialize: true });
        };
        vm.bind = binder;

        var meetingId = new URI(window.location.href).filename(); // using URI library

        console.log("Trying to call ajax with MeetingId=: " + meetingId);
        // Show meeting data once loaded from GET
        vm.loadPageData(meetingId);
    });
});
