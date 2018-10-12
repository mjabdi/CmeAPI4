

$(document).ready(function() {
    $.validator.addMethod("emailRegex",function(value, element) {
        if(/^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/.test( value ))
        { return true;} else{ return false;}
    },"Please enter a valid Email.");

    $.validator.addMethod("phoneRegex",function(value, element) {
        if(/^(07\d{8,12}|447\d{7,11})$/.test( value ))
        { return true;} else{ return false;}
    },"This Phone no is for the UK customers only.");

    $.validator.addMethod("nameRegex", function (value, element) {
        return this.optional(element) || /^([a-zA-Z_-\s]{3,20})$/.test(value);
    }, "Enter valid name");



    $("#w3containerer").validate({
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

            name: {

                required: true,
                nameRegex : true

            },
            phone: {

                required: true,
                phoneRegex: true

            },
            email: {

                required: false,
                email:true

            },

        },

        submitHandler: callMeNow

    });





});


function callMeNow(){

    /*$("#callmenowid").on('submit',(function(e) {
     e.preventDefault();*/

   
    ReqUrl=window.location = self.location.href;
    
    var url = "$server$/api/callme"; // the script where you handle the form input.
    $.ajax({
        url: url,
        type: "POST",
        data:  JSON.stringify({
			reqUrl: ReqUrl,
            token: $('#token').val(),
            name : $('#name').val(),
            email : $('#email').val(),
            phone : $('#phone').val()
		  }),
        headers: {
			'Accept': 'application/json',
			'Content-Type': 'application/json'
		  },

        success :  function(response){
            
            if(response == "OK: Call request sent") {
                $('#inputwidget').remove();
				var div10 = document.createElement("DIV");
				div10.setAttribute("id","callackonnecting");
				document.body.appendChild(div10);
				$('#callackonnecting').html('<div id="id02" class="w3-modal"><div class="w3-modal-content w3-animate-bottom w3-card-4"><header class="w3-container w3-teal"></header><div class="w3-container"><div class="call-me" style="display: block;"><div class="call-back"><div id="callme"><div id="callmeMain"/></div></div></div></div><footer class="w3-container w3-teal footerwg"><div class="h2wg" style="text-align: center;">We are Connecting You</div></footer></div></div></div>');
				document.getElementById('id02').style.display='block'
                setTimeout('window.location = self.location.href',20000);
            }else{
                $('#inputwidget').remove();
					var div10 = document.createElement("DIV");
					div10.setAttribute("id","callackonnecting");
					document.body.appendChild(div10);
					/* $('#callackonnecting').html('<div id="id02" class="w3-modal"><div class="w3-modal-content w3-animate-bottom w3-card-4"><header class="w3-container w3-teal"/><div class="w3-container"><div class="h2wg" style="text-align: center;"/></div>Sorry, something went wrong! Please try again later<h4></div><footer class="w3-container w3-teal footerwg"><h4 style="color:red">Sorry, something went wrong! Please try again later<h4></footer></div></div></div>'); */
					$('#callackonnecting').html('<div id="id02" class="w3-modal"><div class="w3-modal-content w3-animate-bottom w3-card-4"><header class="w3-container w3-teal"/><div class="w3-container"><h4 style="color:red">Sorry, something went wrong! Please try again later<h4><div class="h2wg" style="text-align: center;"/><footer class="w3-container w3-teal footerwg"></footer></div></div></div></div>');
					document.getElementById('id02').style.display='block'
					setTimeout('window.location = self.location.href',4000);
                
            }
        },
        error: function(xhr, statusText, thrownError) {
           
           $('#inputwidget').remove();
					var div10 = document.createElement("DIV");
					div10.setAttribute("id","callackonnecting");
					document.body.appendChild(div10);
					/* $('#callackonnecting').html('<div id="id02" class="w3-modal"><div class="w3-modal-content w3-animate-bottom w3-card-4"><header class="w3-container w3-teal"/><div class="w3-container"><div class="h2wg" style="text-align: center;"/></div>Sorry, something went wrong! Please try again later<h4></div><footer class="w3-container w3-teal footerwg"><h4 style="color:red">Sorry, something went wrong! Please try again later<h4></footer></div></div></div>'); */
					$('#callackonnecting').html('<div id="id02" class="w3-modal"><div class="w3-modal-content w3-animate-bottom w3-card-4"><header class="w3-container w3-teal"/><div class="w3-container"><h4 style="color:red">Sorry, something went wrong! Please try again later<h4><div class="h2wg" style="text-align: center;"/><footer class="w3-container w3-teal footerwg"></footer></div></div></div></div>');
                    document.getElementById('id02').style.display='block';
                   
                    setTimeout("$('#callackonnecting').remove()",4000);
					setTimeout('window.location = self.location.href',4000);
        },
    });
}