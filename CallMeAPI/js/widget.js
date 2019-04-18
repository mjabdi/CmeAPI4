

$(document).ready(function() {
	validateCallMeForm();
 
});


function callMeNow891(){

	
ReqUrl = "'" +  window.location + "'";

$('#btn_ttln_callback').attr('disabled','disabled');
$('#btn_ttln_callback').attr('type','button');
$('#btn_ttln_callback').attr('value','Please wait a moment...');


    var url = "$server$/api/callme"; // the script where you handle the form input.
    $.ajax({
        url: url,
        type: "POST",
        data:  JSON.stringify({
			site : ReqUrl,
            token : $('#talktoleadsnow_token').val(),
            name : $('#talktoleadsnow_name').val(),
            email : $('#talktoleadsnow_email').val(),
            phone : $('#talktoleadsnow_phone').val()
		  }),
        headers: {
			'Accept': 'application/json',
			'Content-Type': 'application/json'
		  },

        success :  function(response){
            hidecallme();
            if(response == "OK: Call request sent") {
			$('#inputwidget').remove();
				var div10 = document.createElement("DIV");
				div10.setAttribute("id","callackonnecting");
				document.body.appendChild(div10);
				$('#callackonnecting').html('<div id="id02" class="w3-modalwg"><div class="w3-modal-contentwg w3-animate-bottomwg w3-card-4wg"><header style="background:#fff !important;" class="w3-containerwg w3-tealwg"><span style="margin:center !important" onclick="closeconnectingbox()" class="w3-buttonwg-close w3-display-topwg">&times;</span></header><div class="w3-containerwg"><div class="call-me" style="display: block;"><div class="call-back"><div id="callme"><div id="callmeMain"/></div></div></div></div><footer style="background:#fff !important;"  class="w3-containerwg w3-tealwg footerwg"><div class="h2wg" style="text-align: center;"><h4> We are connecting you... </h4></div></footer></div></div></div>');
				document.getElementById('id02').style.display='block';
                setTimeout("$('#callackonnecting').remove()",20000);
				
            }else{
			   
                $('#inputwidget').remove();
					var div10 = document.createElement("DIV");
					div10.setAttribute("id","callackonnecting");
					document.body.appendChild(div10);
					$('#callackonnecting').html('<div id="id02" class="w3-modalwg"><div class="w3-modal-contentwg w3-animate-bottomwg w3-card-4wg"><header style="background:#fff !important;" class="w3-containerwg w3-tealwg"><span style="margin:center !important" onclick="closeconnectingbox()" class="w3-buttonwg-close w3-display-topwg">&times;</span></header><div class="w3-containerwg"><h4 style="color:red;text-align:center;">Sorry, something went wrong! Please try again later<h4><div class="h2wg" style="text-align: center;"/><footer  style="background:#fff !important;" class="w3-containerwg w3-tealwg footerwg"/></div></div></div></div>');
					document.getElementById('id02').style.display='block';
					setTimeout("$('#callackonnecting').remove()",4000);
                
            }
        },
        error: function(xhr, statusText, thrownError) {
    
			hidecallme();
			var div10 = document.createElement("DIV");
					div10.setAttribute("id","callackonnecting");
					document.body.appendChild(div10);
					$('#callackonnecting').html('<div id="id02" class="w3-modalwg"><div class="w3-modal-contentwg w3-animate-bottomwg w3-card-4wg"><header style="background:#fff !important;" class="w3-containerwg w3-tealwg"><span style="margin:center !important" onclick="closeconnectingbox()" class="w3-buttonwg-close w3-display-topwg">&times;</span></header><div class="w3-containerwg"><h4 style="color:red;text-align:center;">Sorry, Network Problem! Please try again later<h4><div class="h2wg" style="text-align: center;"/><footer style="background:#fff !important;" class="w3-containerwg w3-tealwg footerwg"/></div></div></div></div>');
					document.getElementById('id02').style.display='block';
					setTimeout("$('#callackonnecting').remove()",2000);
			
        },
    });
	
	
   
}




function validateCallMeForm(){
	// hidecallme();
	$.validator.addMethod("emailRegex",function(value, element) {
        if(/^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/.test( value ))
        { return true;} else{ return false;}
    },"Please enter a valid Email.");

    $.validator.addMethod("phoneRegex",function(value , element) {
        
        var pos = value.trim().substr(0, 1);
        if (pos != '0')
        {
            value = '0' + value;
        }

        if (value.substr(0, 3) == '070')
            return false;

        if(/^(07\d{8,12}|01\d{8,12}|02\d{8,12})$/.test( value ))
        { return true;} else{ return false;}
    },"This Phone no is for the UK customers only.");

    $.validator.addMethod("nameRegex", function (value, element) {
        return this.optional(element) || /^([a-zA-Z_-\s]{3,20})$/.test(value);
    }, "Enter valid name");



    $("#w3containererwg").validate({
        errorElement: 'span',
        errorClass: 'helpError',
        highlight: function(element, errorClass, validClass) {
            $(element).addClass("errorClass");
        },
        unhighlight: function(element, errorClass, validClass) {
            $(element).removeClass("errorClass");
        },
        errorPlacement: function (error, element) {
            error.insertAfter(element);
        },

        rules: {

            talktoleadsnowname: {

                required: true,
                nameRegex : true

            },
            talktoleadsnowphone: {

                required: true,
                phoneRegex: true

            },
            talktoleadsnowemail: {

                required: true,
                email:true

            },

        },

        submitHandler: callMeNow891
    });
}


