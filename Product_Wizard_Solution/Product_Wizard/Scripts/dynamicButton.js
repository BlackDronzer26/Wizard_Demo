$(document).ready(function(){

    $("#add").click(function(e){
        $('#items').append('<input type ="text" name="name"><br><input type ="text" name="address"><br><input type ="text" name="email"> <input type="button" value="delete" id="delete"/>');
    });

    $('#delete').click(function(e){
        $(this).parent('div').remove;
    });

});
