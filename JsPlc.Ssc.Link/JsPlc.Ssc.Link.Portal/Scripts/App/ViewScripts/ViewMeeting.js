require(["jquery", "knockout", "moment", "bootstrap-datepicker", "bootstrap-datepickerGB", "URI", "underscore", "common", "helpers", "autogrow"],
    function ($, ko, moment, datepicker, datePickerGb, URI, _, common, helpers, autogrow) {

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
                MeetingDate: data.MeetingDate,
                ColleagueSignedOffDate: data.ColleagueSignedOffDate,
                ManagerSignedOffDate: data.ManagerSignedOffDate,
                MeetingId: data.MeetingId,
                ColleagueInitiated: data.ColleagueInitiated,
                //Completed: data.Status,
                ColleagueSignOff: data.ColleagueSignOff,
                ManagerSignOff: data.ManagerSignOff,
                LookingBackQuestions: [],
                LookingFwdQuestions: [],
                DrivingDevelopmentQuestions: [],
                InANutShellQuestions: [],
                Questions: [],
                DownloadUrl: common.getSiteRoot() + 'pdf/DownloadFromDb/?MeetingId=' + data.MeetingId
            };

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
                        // $('#msgs').html("Fetch success");

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
