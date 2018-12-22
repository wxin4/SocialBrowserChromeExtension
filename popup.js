//javascript must be in separate file for chrome to allow it
//onclicks must be in here and not in the html file

var url = null;
var domain = null;

//UNCOMMENT WHEN UPLOADING TO CHROME.
//This works in chrome but not browser debugging because it is specific to extensions.  Gets current tab
//When browser debugging it just returns null for url
/*chrome.tabs.query({'active': true, 'lastFocusedWindow': true}, function (tabs) {
    url = tabs[0].url;
}); */
chrome.tabs.getSelected(null, function (tab) {
    url = new URL(tab.url)
    domain = url.hostname
});

//displays keyword form
function firstLike() {

    document.getElementById("keyword_text").style.display = "list-item";

    setTimeout(function () {
        //var keyword = "nil";

        var stringToDisplay = "" // Begins the string of URLS to display

        var xhr = new XMLHttpRequest();
        //currently posting to old server/database
        xhr.open("POST", 'http://localhost:49184/WebService.asmx/getSimilarWebsites', true);

        //Send the proper header information along with the request
        xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

        xhr.onreadystatechange = function () // This executes when response is gotten
        {
            if (this.readyState == XMLHttpRequest.DONE && this.status == 200) {
                var result = JSON.parse(xhr.response);
                console.log(xhr.response);
                console.log(result);

                var array = [];
                var count = 0;
                //document.getElementById("result").innerHTML = "Similar websites are:  " + result;
                for (website in result) {
                    stringToDisplay = JSON.stringify(result[website]['websiteName']);
                    stringToDisplay = stringToDisplay.replace(/"/g, '');

                    array.push(stringToDisplay);
                    count++;
                    console.log("printing:" + JSON.stringify(result[website]['websiteName']));
                }

                var id = "result";

                /*for (var i = 0; i <= 5; i++) {
                    id = "result" + (i + 1).toString();
                    document.getElementById(id).innerHTML = array[i].link("https://" + array[i]);
                }*/

                /*document.getElementById("result2").innerHTML = array[1].link("https://" + array[1]);
                document.getElementById("result3").innerHTML = array[2].link("https://" + array[2]);
                document.getElementById("result4").innerHTML = array[3].link("https://" + array[3]);
                document.getElementById("result5").innerHTML = array[4].link("https://" + array[4]);
				document.getElementById("result6").innerHTML = array[5].link("https://" + array[5]);*/


                //document.getElementById("result").innerHTML = "Similar websites are:  " + JSON.stringify(result[website]['WebsiteName']);

                //document.getElementById("result").innerHTML = "Similar websites are:  " + JSON.stringify(result);		
            }

        }

        xhr.send("key=nil" + "&domain=" + domain); // What to send, happends after above for some reason

        //document.getElementById("result").innerHTML = "Similar websites are:  " + result;

        document.getElementById("submit_test").style.display = "block";

    }, 500);
    
}

document.getElementById("like_button").onclick = firstLike;

//displays url and keyword together
function submit_key() {
    setTimeout(function () {

        var keyword = document.getElementById("keyword").value;

        var stringToDisplay = "" // Begins the string of URLS to display

        var xhr = new XMLHttpRequest();
        xhr.open("POST", 'http://localhost:49184/WebService.asmx/getSimilarWebsites', true);

        //Send the proper header information along with the request
        xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

        xhr.onreadystatechange = function () // This executes when response is gotten
        {
            if (this.readyState == XMLHttpRequest.DONE && this.status == 200) {
                var result = JSON.parse(xhr.response);
                console.log(xhr.response);
                console.log(result);

                var array = [];
                var count = 0;
                //document.getElementById("result").innerHTML = "Similar websites are:  " + result;
                for (website in result) {
                    stringToDisplay = JSON.stringify(result[website]['websiteName']);
                    stringToDisplay = stringToDisplay.replace(/"/g, '');

                    array.push(stringToDisplay);
                    count++;
                    console.log("printing:" + JSON.stringify(result[website]['websiteName']));
                }                              
                
                var id = "result";

                
                for (var i = 1; i <= 6; i++) {
                    id = "result" + i.toString();
                    document.getElementById(id).innerHTML = ""
                }
                
                for(var i = 0; i <= 5; i++)
                {
                	id = "result" + (i + 1).toString();
                	document.getElementById(id).innerHTML = array[i].link("https://" + array[i]);
                }
                
                /*document.getElementById("result2").innerHTML = array[1].link("https://" + array[1]);
                document.getElementById("result3").innerHTML = array[2].link("https://" + array[2]);
                document.getElementById("result4").innerHTML = array[3].link("https://" + array[3]);
                document.getElementById("result5").innerHTML = array[4].link("https://" + array[4]);
				document.getElementById("result6").innerHTML = array[5].link("https://" + array[5]);*/
                
				
                //document.getElementById("result").innerHTML = "Similar websites are:  " + JSON.stringify(result[website]['WebsiteName']);

                //document.getElementById("result").innerHTML = "Similar websites are:  " + JSON.stringify(result);		
            }

        }

        xhr.send("key=" + keyword + "&domain=" + domain); // What to send, happends after above for some reason

        //document.getElementById("result").innerHTML = "Similar websites are:  " + result;

        document.getElementById("submit_test").style.display = "block";

    }, 500);
}


document.getElementById("key_button").onclick = submit_key;

