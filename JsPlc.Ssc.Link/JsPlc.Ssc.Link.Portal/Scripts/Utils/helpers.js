define(['jquery'], function($) {
    return function() {

        var queryStringHelpers = {

            // Reads query string and returns an object with keys and values
            getQueryParams: function(str) {
                var qso = {};
                var qs = (str || document.location.search);

                // Check for an empty querystring
                if (qs == "") {
                    return qso;
                }

                // Normalize the querystring
                qs = qs.replace(/(^\?)/, '')
                    .replace(/;/g, '&');
                while (qs.indexOf("&&") != -1) {
                    qs = qs.replace(/&&/g, '&');
                }
                qs = qs.replace(/([\&]+$)/, '');

                // Break the querystring into parts
                qs = qs.split("&");

                // Build the querystring object
                for (var i = 0; i < qs.length; i++) {
                    var qi = qs[i].split("=");
                    qi = $.map(qi, function(n) { return decodeURIComponent(n); });
                    if (qso[qi[0]] != undefined) {

                        // If a key already exists then make this an object
                        if (typeof(qso[qi[0]]) == "string") {
                            var temp = qso[qi[0]];
                            if (qi[1] == "") {
                                qi[1] = null;
                            }
                            //console.log("Duplicate key: ["+qi[0]+"]["+qi[1]+"]");
                            qso[qi[0]] = [];
                            qso[qi[0]].push(temp);
                            qso[qi[0]].push(qi[1]);

                        } else if (typeof(qso[qi[0]]) == "object") {
                            if (qi[1] == "") {
                                qi[1] = null;
                            }
                            //console.log("Duplicate key: ["+qi[0]+"]["+qi[1]+"]");
                            qso[qi[0]].push(qi[1]);
                        }
                    } else {
                        // If no key exists just set it as a string
                        if (qi[1] == "") {
                            qi[1] = null;
                        }
                        //console.log("New key: ["+qi[0]+"]["+qi[1]+"]");
                        qso[qi[0]] = qi[1];
                    }
                }
                return qso;
            }
        }
        return {
            queryStringHelpers: queryStringHelpers
        }
    }();
});

// Testing 
//var qs = "?foo=bar&foo=boo&roo=bar;bee=bop;=ghost;=ghost2;&;checkbox%5B%5D=b1;checkbox%5B%5D=b2;dd=;http=http%3A%2F%2Fw3schools.com%2Fmy%20test.asp%3Fname%3Dst%C3%A5le%26car%3Dsaab&http=http%3A%2F%2Fw3schools2.com%2Fmy%20test.asp%3Fname%3Dst%C3%A5le%26car%3Dsaab";
////var qs = "?=&=";
////var qs = ""

//var results = qsParams(qs);

//$("#results").html(JSON.stringify(results, null, 2));

