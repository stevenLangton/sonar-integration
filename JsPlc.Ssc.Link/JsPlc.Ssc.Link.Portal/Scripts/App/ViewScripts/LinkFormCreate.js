define(["jquery", "knockout", "moment", "bootstrap-datepicker", "bootstrap-datepickerGB", "underscore", "common", "helpers", "URI"],
function ($, ko, moment, datepicker, datePickerGb, _, common, helpers, URI) {
    var getMessages = function (crudMode) {
        switch (crudMode) {
            case "Create":
                return {
                    success: "Meeting created",
                    failure: "Meeting creation failed"
                };
                break;
            case "Edit":
                return {
                    success: "Meeting updated",
                    failure: "Meeting update failed"
                };
                break;
            default:
                return undefined;
        };
    };

    function PageViewModel() {

        var self = this;

        self.dataModel = ko.observable(""); // LinkForm object
        self.dataAvailable = ko.observable(false);

        self.bind = function () { };

        var meetingView;
        var buildViewModels = function (data) {
            moment.locale("en-gb"); // Set Locale for moment (aka moment.locale("en-gb"))
            // Refer to Model values http://localhost/JsPlc.Ssc.Link.Service/api/Meetings/?colleagueId=E001
            var meetingDate = moment(data.MeetingDate).format("L"); // we get dd/mm/yyyy
            meetingView = {
                EmployeeId: data.colleagueId,
                ColleagueId: data.ColleagueId,
                ManagerId: data.ManagerId,
                PeriodId: data.PeriodId,
                ManagerName: data.ManagerName,
                ColleagueName: data.ColleagueName,
                MeetingDate: meetingDate,
                MeetingId: data.MeetingId,
                ColleagueSignOff: data.ColleagueSignOff,
                ManagerSignOff: data.ManagerSignOff,
                ColleagueInitiated: data.ColleagueInitiated,
                //Completed: data.Status,
                LookingBackQuestions: [],
                LookingFwdQuestions: [],
                Questions: []
            };

            ko.utils.arrayForEach(data.Questions, function (ques) {
                if (!ques.ColleagueComment)
                { ques.ColleagueComment = ""; } // NOTE TextArea and other input elements need to be bound to "value" not just text, otherwise we dont see user changes in model
                if (!ques.ManagerComment)
                { ques.ManagerComment = ""; }
            });

            //meetingView.LookingBackQuestions = data.Questions.slice(0, 2);
            //meetingView.LookingFwdQuestions = data.Questions.slice(2, 5);
            // split questions by type (not by index)
            meetingView.LookingBackQuestions = _.select(data.Questions, function (ques) { return ques.QuestionType == 'b'; });
            meetingView.LookingFwdQuestions = _.select(data.Questions, function (ques) { return ques.QuestionType == 'f'; });
            meetingView.Questions = data.Questions;

            self.dataModel(meetingView);
        };

        // ### GET LinkForm Data (assume there is data, it will show up), we may have to build a Get method which returns a blank Link Meeting template
        self.loadPageData = function (promise) {
            //$.ajax({
            //    url: common.getSiteRoot() + "LinkForm/GetLinkForm/?" + "colleagueId" + "=" + colleagueId, // Hardcoded Meeting GET for now...
            //    method: "GET"
            //})
                promise.done(function (data, textStatus, jqXhr) {
                    if (data == "Error") {
                        $('#msgs').html("Error - no data");
                        self.dataAvailable(false);
                    }
                    else {
                        // $('#msgs').html("Fetch success");

                        buildViewModels(data);
                        self.dataAvailable(true);
                        self.bind();
                    }
                })
                .fail(function () {
                    self.dataAvailable(false);
                    $('#msgs').html("Error occured");

                    // if cannot load LinkMeeting for the given period
                });
        }

        // ### POST LinkForm data back to server.
        self.saveLinkForm = function () {
            console.log("Form data:" + $('#myform').serialize());
            console.log("Form data(json):" + ko.toJSON(self.dataModel));
            console.log("Self.DataModel:" + self.dataModel());
            // copy all LookingFwdQuestions and LookingBackQuestions to Questions Array
            var data = self.dataModel();

            // Manage bool binding to MeetingStatus.
            // Values depend on MeetingStatus object in C#
            data.ColleagueSignOff = (data.ColleagueSignOff == 0 || data.ColleagueSignOff == false) ? "InComplete" : "Completed";
            data.ManagerSignOff = (data.ManagerSignOff == 0 || data.ManagerSignOff == false) ? "InComplete" : "Completed";

            console.log("Postback meetingDate :" + data.MeetingDate);

            data.Questions = [];
            ko.utils.arrayForEach(data.LookingBackQuestions, function (ques) {
                data.Questions.push(ques);
            });
            ko.utils.arrayForEach(data.LookingFwdQuestions, function (ques) {
                data.Questions.push(ques);
            });

            // end copy questions to model.
            var messages = getMessages(self.crudMode);

            $.ajax({
                url: common.getSiteRoot() + "LinkForm/PostLinkForm",
                method: "POST",
                data: self.dataModel(),
                contentType: 'application/x-www-form-urlencoded; charset=utf-8', // 'application/json'
            })
                .done(function (response, textStatus, jqXhr) {

                    if (response.JsonStatusCode.CustomStatusCode == "ApiSuccess") {
                        window.alert(messages.success);

                        // Redirect for colleagues initiated create meeting success.. (my Link Report page)
                        if (data.ColleagueInitiated) {
                            window.location.href = common.getSiteRoot() + "Home/LinkReport";
                        } else {
                            window.location.href = common.getSiteRoot() + "Team";
                        }
                    }
                    else if (response.JsonStatusCode.CustomStatusCode == "ApiFail") {
                        window.alert(messages.failure);
                        $('#msgs').html("<strong>" + messages.failure + " : " + response + "</strong>");
                    }
                    else { // UI validation errors
                        displayErrors(response.ModelErrors);
                    }
                    // msg success and redirect to another page if needed
                    // Define a standard message format for Post(aka Create) Response returned,
                    //  which should contain location header for created resource.
                    // We should redirect to that returned resource location URL.
                    // Incase of new Creation, we will get a MeetingId back (which was 0 initially).
                })
                .fail(function (jqXhr, textStatus, errorThrown) {
                    // msg failure
                    window.alert(messages.failure);
                    $('#msgs').html("<strong>" + messages.failure + ":" + errorThrown + "</strong>");
                });
        }

        ///
        self.downloadLinkFormAsPdf = function () {
            var MeetingView = self.dataModel();//Not quite the same
            var param = { MeetingData: MeetingView };
            //var $promise = common.callService("post", "Pdf/MakeFromJson", param);
            $.ajax({
                url: common.getSiteRoot() + "Pdf/MakeFromJson",
                method: "POST",
                data: param
            })
                .done(function (data) {
                    if (data.success) {
                        //window.location.href = "http://localhost/Pdf/DownloadPdf/" + "?fName=" + data.fName;
                        window.location.href = common.getSiteRoot() + "Pdf/DownloadPdf/" + "?fName=" + data.fName;
                    }
                });
        };

        // Parses response.ModelErrors dictionary
        var displayErrors = function (errors) {
            var errorsList = "";
            for (var i = 0; i < errors.length; i++) {
                errorsList = errorsList + "<li>" + errors[i].Value[0] + "</li>";
            }
            $("#msgs").html("<ul class='linkErrorMsg'>" + errorsList + "</ul>");
            window.alert("VALIDATION ERRORS, please see top of screen for remedial action.");
        }

        self.getMeetingView = function () {
            return self.dataModel();
        }

        //Luan


        //
        self.crudMode = "";
        self.getDataForMeeting = function () {
            //Check CRUD mode
            debugger;
            if (window.location.href.search("LinkForm/ViewMeeting") > 0) {
                self.crudMode = "View";
            }
            else if (window.location.href.search("LinkForm/Create") > 0) {
                self.crudMode = "Create";

                var url = "LinkForm/GetLinkForm";

                var pageQueryParams = helpers.queryStringHelpers.getQueryParams(window.location.search);
                var empId = pageQueryParams["colleagueId"];
                var jsonArgs = { colleagueId: empId };

                var $promise = common.callService("get", url, jsonArgs);
                self.loadPageData($promise);

            }
            else if (window.location.href.search("LinkForm/Edit") > 0) {
                self.crudMode = "Edit";

                var url = "LinkForm/GetMeetingView";

                var meetingId = new URI(window.location.href).filename(); // using URI library
                var jsonArgs = { meetingId: meetingId };

                var $promise = common.callServerAction("get", url, jsonArgs);
                self.loadPageData($promise);
            }
        };

        self.confirmCheckbox = function (data, event) {
            if (event.currentTarget.checked === true) {
                var box = confirm("Are you sure you want to complete this form?");
                if (box == true)
                    return true;
                else
                    return false;
            };

            return true;
        }
        //End Luan
    };


    $(document).ready(function () {
        moment.locale("en-gb"); // Set Locale for moment (aka moment.locale("en-gb"))

        // http://stackoverflow.com/questions/26487765/bootstrap-datepicker-set-language-globally
        // set locales
        try {
            $.fn.datepicker.defaults.language = 'en-GB';
            $('.datepicker').datepicker({ language: "en-GB", dateFormat: 'dd/mm/yyyy', orientation: 'auto top' });
        } catch (e) {

        }

        // knockout locale based date formatting - ko.observable(dateFormat(date, "dd/mm/yyyy"));
        // bootstrap datepicker formatting =  $("#meetingDatePicker").datepicker({dateFormat: 'dd/mm/yy'});

        var vm = new PageViewModel();

        var binder = function () {
            ko.applyBindings(vm, $("#linkpage")[0]); // important - we have to refer to div element with index 0.
            // set locales
            try {
                $('.datepicker').datepicker({ language: "en-GB", dateFormat: 'dd/mm/yyyy' });
            } catch (e) {

            }
        };
        vm.bind = binder;

        //var pageQueryParams = helpers.queryStringHelpers.getQueryParams(window.location.search);
        //var empId = pageQueryParams["colleagueId"];
        ////var periodId = pageQueryParams["periodId"];
        ////console.log("EmpID=" + empId + ", Full QueryString as json: " + JSON.stringify(pageQueryParams, null, 2));

        //// Show meeting data once loaded from GET
        //vm.loadPageData(empId);
        vm.getDataForMeeting();

        var MeetingViewJsonForPdf = vm.getMeetingView();
    });
});