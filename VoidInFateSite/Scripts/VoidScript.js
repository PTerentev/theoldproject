
    $(document).ready(function() {

        $("#Add").on("click", addRecent);
        $("#Addimage").on("click", addImage);

    });
    var sumrecent = 0;
  

    function addRecent() {
        var str = $("#Text").val();
        var str1 = $("#FullText").val();
        var str2 = $("#Caption").val();
        var str3 = $("#Author").val();
        var mas = [str2 ,  str ,  str1 , str3 ];
        var file1 = document.getElementById('LinkToSmallImage').files[0];
        var file2 = document.getElementById('LinkToLargeImage').files[0];
        
     alert("test");
        jQuery.ajax
        ({
            url: 'api/Recents/NewRecent/',
            type: 'POST',
            dataType: "json",
         data: JSON.stringify(mas),
         contentType: "application/json;charset=utf-8",
         success: function (ok) {
             alert('success' + ok);
             jQuery.ajax
             ({
                 url: "api/Recents/LinkToSmallImage/" + ok,
                 type: "POST",
                 dataType: "json",
                 data: file1,

                 success: function () {
                     alert('success' );
                 },
                 error: function (issies) {
                     alert('Sorry.' + issies);
                 },
                 contentType: "application/octet-stream",
                 processData: false



             });
             jQuery.ajax
             ({
                 url: "api/Recents/LinkToLargeImage/" + ok,
                 type: "POST",
                 dataType: "json",
                 data: file2,

                 success: function () {
                     alert('success' );
                 },
                 error: function (issies) {
                     alert('Sorry.' + issies);
                 },
                 contentType: "application/octet-stream",
                 processData: false



             });
         },
         error: function (issies) {
             alert('Sorry.' + issies);
         }
         
         



     });
        
       
        


    };
    function addImage() {
        alert("test1");
        var str5 = $("#Name").val();
        var str6 = $("#Code").val();
        var mas = [str5, str6];
        var file = document.getElementById('image').files[0];

        alert("test");
        jQuery.ajax
        ({
                url: 'api/Recents/NewImage/',
            type: 'POST',
            dataType: "json",
            data: JSON.stringify(mas),
            contentType: "application/json;charset=utf-8",
            success: function (ok) {
                alert('success' + ok);
                jQuery.ajax
                ({
                    url: "api/Recents/Image/" + ok,
                    type: "POST",
                    dataType: "json",
                    data: file,

                    success: function () {
                        alert('success');
                    },
                    error: function (issies) {
                        alert('Sorry.' + issies);
                    },
                    contentType: "application/octet-stream",
                    processData: false



                });
               
            },
            error: function (issies) {
                alert('Sorry.' + issies);
            }





        });





    };

   