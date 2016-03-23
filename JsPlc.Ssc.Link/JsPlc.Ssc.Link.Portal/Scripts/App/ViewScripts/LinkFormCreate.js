define(["jquery", "knockout", "moment", "bootstrap-datepicker", "bootstrap-datepickerGB", "datetimepicker", "underscore", "common", "helpers", "URI", "toastr", "autogrow"],
function ($, ko, moment, datepicker, datePickerGb, datetimepicker, _, common, helpers, URI, toastr, autogrow) {
    var getMessages = function (crudMode) {
        switch (crudMode) {
            case "Create":
                return {
                    success: "Conversation successfully created",
                    failure: "Conversation creation failed"
                };
            case "Edit":
                return {
                    success: "Conversation successfully updated",
                    failure: "Conversation update failed"
                };
            default:
                return undefined;
        }
    };

    var adjustBreadcrumbs = function (meetingOwnerColleagueId) {
        //Breadcrumbs reflect the static positions of web pages on the website with each page having a unique position.
        //Because there are many ways to get to the "view conversation" page even though it has only a single location (or mvc route or address).
        //But we wish to show different breadcrumbs for it depending on "My Team" view or "My Development". Hence this manual adjustment here.

        var colleagueId = common.getUserInfo().colleagueId;
        if (colleagueId === meetingOwnerColleagueId) {
            //Own meeting
            $anchor = $("#breadcrumbs a:contains('MY TEAM MEMBERS')");
            $anchor.replaceWith("<a href='/Home/LinkMeetings'>LINK CONVERSATIONS</a>");

            $anchor = $("#breadcrumbs a:contains('MY TEAM')");
            $anchor.replaceWith("<a href='/Home/Index'>MY DEVELOPMENT</a>");
        }

    };

    function PageViewModel() {

        var self = this;

        self.dataModel = ko.observable(""); // LinkForm object
        self.dataAvailable = ko.observable(false);

        self.bind = function () { };

        var meetingView;
        var buildViewModels = function (data) {
            moment.locale("en-gb"); // Set Locale for moment (aka moment.locale("en-gb"))

            debugger;

            // Refer to Model values http://localhost/JsPlc.Ssc.Link.Service/api/Meetings/?colleagueId=E001
            var meetingDate = moment(data.MeetingDate).format("L"); // we get dd/mm/yyyy
            var meetingTime = moment(data.MeetingDate).format("HH:mm a"); // we get HH:mm

            var initDate = moment(data.MeetingDate).isValid() ? new Date(moment(data.MeetingDate).toISOString()) : new Date();

            // Init the calendar to the data.MeetingDate
            $('.datepicker').datepicker("setDate", initDate);
            debugger;

            meetingView = {
                EmployeeId: data.colleagueId,
                ColleagueId: data.ColleagueId,
                ManagerId: data.ManagerId,
                PeriodId: data.PeriodId,
                ManagerName: data.ManagerName,
                ColleagueName: data.ColleagueName,
                Colleague: data.Colleague,
                Manager: data.Manager,
                ColleagueSignedOffDate: data.ColleagueSignedOffDate, 
                ManagerSignedOffDate: data.ManagerSignedOffDate,
                MeetingDate: meetingDate,  // No specific need for Signoff dates on this view
                MeetingTime: meetingTime,
                MeetingId: data.MeetingId,
                ColleagueSignOff: data.ColleagueSignOff,
                ManagerSignOff: data.ManagerSignOff,
                IsColleagueView: data.ColleagueInitiated,
                SharingDate: data.SharingDate,
                SharingStatus: data.SharingStatus,
                //Completed: data.Status,
                LookingBackQuestions: [],
                LookingFwdQuestions: [],
                DrivingDevelopmentQuestions: [],
                InANutShellQuestions: [],
                Questions: []
            };
            // Oh the IE pain
            $("input[name='CompletedMgr']").prop('checked', (data.ManagerSignOff ==1) ? true: false);
            $("input[name='CompletedColleague']").prop('checked', (data.ColleagueSignOff == 1) ? true : false);
            $("input[name='CheckboxShared']").prop('checked', (data.SharingStatus == 1) ? true : false);

            //if (data.ManagerSignOff == 1 || data.ManagerSignOff == true) meetingView.ManagerSignOff = true;
            //else meetingView.ManagerSignOff = false;


            ko.utils.arrayForEach(data.Questions, function (ques) {
                if (!ques.ColleagueComment)
                { ques.ColleagueComment = ""; } // NOTE TextArea and other input elements need to be bound to "value" not just text, otherwise we dont see user changes in model
                if (!ques.ManagerComment)
                { ques.ManagerComment = ""; }

                //ques.UpdCharCount = function (textAreaId, charSpanId) {
                //    var txtLen = $(textAreaId).val().length; // bit inaccurate this..
                //    var charCount = (5000 - txtLen-10);
                //    $(charSpanId).html(charCount + " chars remaining");
                //};

            });
            // split questions by type (not by index)

            meetingView.LookingBackQuestions = _.select(data.Questions, function (ques) { return ques.QuestionType == 'LOOKING BACK'; });
            meetingView.LookingFwdQuestions = _.select(data.Questions, function (ques) { return ques.QuestionType == 'LOOKING FORWARD'; });
            meetingView.DrivingDevelopmentQuestions = _.select(data.Questions, function (ques) { return ques.QuestionType == 'DRIVING MY DEVELOPMENT'; });
            meetingView.InANutShellQuestions = _.select(data.Questions, function (ques) { return ques.QuestionType == 'IN A NUTSHELL'; });
            meetingView.Questions = data.Questions;

            self.dataModel(meetingView);
            adjustBreadcrumbs(self.dataModel().ColleagueId);
        };

        self.formatDateMonthDYHM = function (dateObj) {
            if (!dateObj) return '-';
            var formattedString = moment(dateObj, "").format('dddd, MMMM Do YYYY [at] HH:mma');
            return formattedString;
        }

        // ### GET LinkForm Data (assume there is data, it will show up), we may have to build a Get method which returns a blank Link Meeting template
        self.loadPageData = function (promise) {
            promise.done(function (data, textStatus, jqXhr) {
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
                self.bind();
                $('#msgs').html("Error occured");
            });
        }

        self.IsDataAvailable = function () {
            debugger;
            var retval = self.dataAvailable() ? 1 : 0;
            return retval;
        }
        // ### POST LinkForm data back to server.
        self.saveLinkForm = function () {
            moment.locale("en-gb");

            var data = self.dataModel();

            // Manage bool binding to MeetingStatus.
            // Values depend on MeetingStatus object in C#
            data.ColleagueSignOff = $("input[name='CompletedColleague']").prop('checked');
            data.ColleagueSignOff = data.ColleagueSignOff && (data.ColleagueSignOff == 1 || data.ColleagueSignOff == true) ? "Completed" : "InComplete";

            data.ManagerSignOff = $("input[name='CompletedMgr']").prop('checked');
            data.ManagerSignOff = data.ManagerSignOff && (data.ManagerSignOff == 1 || data.ManagerSignOff == true) ? "Completed" : "InComplete";

            data.SharingStatus = $("input[name='CheckboxShared']").prop('checked');
            data.SharingStatus = data.SharingStatus && (data.SharingStatus == 1 || data.SharingStatus == true) ? "Shared" : "NotShared";

            //var ukDate = moment(data.MeetingDate, "DD/MM/YYYY");
            //var yyyymmdd = ukDate.toISOString();
            //console.log("Proposed postback meetingDate (yyyy-mm-ddThh:mm:ss.xxxZ):" + yyyymmdd);
            //data.MeetingDate = yyyymmdd;
            debugger;
            if (moment(data.MeetingDate, common.uiDateFormat).isValid()) {
                data.MeetingDate = moment(data.MeetingDate, common.uiDateFormat).format("YYYY-MM-DD")
                    + ' ' + moment(data.MeetingTime, "HH:mm a").format("HH:mm a");
            } else if (moment(data.MeetingDate, "YYYY-MM-DD").isValid() === false) {
                data.MeetingDate = "";
                data.MeetingTime = "";
            }
            // These 2 are set server side, we dont set it client side, Need this statement for Model validation to pass..
            data.ColleagueSignedOffDate = "";
            data.ManagerSignedOffDate = "";
            data.SharingDate = "";

            data.Questions = [];
            ko.utils.arrayForEach(data.LookingBackQuestions, function (ques) {
                data.Questions.push(ques);
            });
            ko.utils.arrayForEach(data.LookingFwdQuestions, function (ques) {
                data.Questions.push(ques);
            });
            ko.utils.arrayForEach(data.DrivingDevelopmentQuestions, function (ques) {
                data.Questions.push(ques);
            });
            ko.utils.arrayForEach(data.InANutShellQuestions, function (ques) {
                data.Questions.push(ques);
            });

            // end copy questions to model.
            var messages = getMessages(self.crudMode);

            $.ajax({
                url: common.siteUrls.postLink,
                method: "POST",
                data: data,
                contentType: 'application/x-www-form-urlencoded; charset=utf-8', // 'application/json'
            })
                .done(function (response, textStatus, jqXhr) {

                    if (response.JsonStatusCode.CustomStatusCode == "ApiSuccess") {
                        toastr.info(messages.success);
                        // Redirect for colleagues initiated create meeting success.. (my Link Report page)
                        if (data.IsColleagueView) {
                            helpers.utils.redirectWithDelay(common.siteUrls.meetingsPage);
                        } else {
                            helpers.utils.redirectWithDelay(common.siteUrls.teamPage);
                        }
                    }
                    else if (response.JsonStatusCode.CustomStatusCode == "ApiFail") {
                        toastr.error(messages.failure);
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
                    toastr.error(messages.failure);
                    $('#msgs').html("<strong>" + messages.failure + ":" + errorThrown + "</strong>");
                });
        }

        // Parses response.ModelErrors dictionary
        var displayErrors = function (errors) {
            var errorsList = "";
            for (var i = 0; i < errors.length; i++) {
                errorsList = errorsList + "<li>" + errors[i].Value[0] + "</li>";
            }
            $("#msgs").html("<ul class='linkErrorMsg'>" + errorsList + "</ul>");
            toastr.error("VALIDATION ERRORS, please see top of screen for remedial action.");
        }

        self.utils = helpers.utils;

        //
        self.crudMode = "";
        self.getDataForMeeting = function () {
            //Check CRUD mode
            //debugger;
            var url;
            var jsonArgs;
            var $promise;
            if (window.location.href.search(common.linkUrls.viewMeeting) > 0) {
                self.crudMode = "View";
            }
            else if (window.location.href.search(common.linkUrls.createMeeting) > 0) {
                self.crudMode = "Create";

                url = common.linkUrls.getLinkForm;

                var pageQueryParams = helpers.queryStringHelpers.getQueryParams(window.location.search);
                var empId = pageQueryParams["colleagueId"];
                jsonArgs = { colleagueId: empId };

                $promise = common.callServerAction("get", url, jsonArgs);
                self.loadPageData($promise);

            }
            else if (window.location.href.search(common.linkUrls.editMeeting) > 0) {
                self.crudMode = "Edit";

                url = common.linkUrls.getMeeting; //"LinkForm/GetMeetingForEdit";

                var meetingId = new URI(window.location.href).filename(); // using URI library
                jsonArgs = { meetingId: meetingId };

                $promise = common.callServerAction("get", url, jsonArgs);
                self.loadPageData($promise);
            }
        };

        self.confirmCheckbox = function (data, event) {
            //debugger;
            //if (event.currentTarget.checked === true) {
            //    var box = confirm("Are you sure you want to complete this form?");
            //    if (box == true)
            //        return true;
            //    else
            //        event.currentTarget.checked = false;
            //        return true;
            //} else {
            //    event.currentTarget.checked = false;
            //}
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
            $('.datepicker').datepicker({ language: "en-GB", dateFormat: 'dd/mm/yyyy', orientation: 'auto top', autoclose: true });
            $('#timepicker1').timepicker();
        } catch (e) {

        }

        // knockout locale based date formatting - ko.observable(dateFormat(date, "dd/mm/yyyy"));
        // bootstrap datepicker formatting =  $("#meetingDatePicker").datepicker({dateFormat: 'dd/mm/yy'});

        var vm = new PageViewModel();

        var binder = function () {
            ko.applyBindings(vm, $("#linkpage")[0]); // important - we have to refer to div element with index 0.
            $('textarea').autogrow({ onInitialize: true });
            // set locales
            try {
                $('.datepicker').datepicker({ language: "en-GB", dateFormat: 'dd/mm/yyyy' });
                $('#timepicker1').timepicker();
            } catch (ex) {

            }
        };
        vm.bind = binder;

        //// Show meeting data once loaded from GET
        //vm.loadPageData(empId);
        vm.getDataForMeeting();

        if (!jQuery || jQuery == undefined) window.jQuery = $;
    });
});



