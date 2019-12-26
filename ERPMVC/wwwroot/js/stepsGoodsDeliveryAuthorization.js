$(".tab-wizard").steps({
    headerTag: "h6",
    bodyTag: "section",
    transitionEffect: "fade",
    titleTemplate: '<span class="step">#index#</span> #title#',
    labels: {
        next: 'Siguiente <i class="fa fa-chevron-right"></i>',
        previous: '<i class="fa fa-chevron-left"></i> Anterior',
        finish: "Finalizar"
    }, 
    onStepChanging: function (event, currentIndex, newIndex) {
          console.log('Current Index'+currentIndex);
         //  return $("#CustomerId").val() == 
       // return currentIndex > newIndex || !(3 === newIndex && Number($("#CustomerIdAsociarRecibo").val()) === '') && (currentIndex < newIndex && (form.find(".body:eq(" + newIndex + ") label.error").remove(), form.find(".body:eq(" + newIndex + ") .error").removeClass("error")), form.validate().settings.ignore = ":disabled,:hidden", form.valid())
          return currentIndex > newIndex || !(3 === newIndex && Number($("#age-2").val()) < 18) && (currentIndex < newIndex && (form.find(".body:eq(" + newIndex + ") label.error").remove(), form.find(".body:eq(" + newIndex + ") .error").removeClass("error")), form.validate().settings.ignore = ":disabled,:hidden", form.valid())
    }
   , onFinished: function (event, currentIndex) {
        var form = $(this);     
        form.submit();        
    }
});


var form = $(".validation-wizard").show();

$(".validation-wizard").steps({
    headerTag: "h6"
    , bodyTag: "section"
    , transitionEffect: "fade"
    , titleTemplate: '<span class="step">#index#</span> #title#'
    , labels: {
        finish: "Finalizar",
        next: 'Siguiente <i class="fa fa-chevron-right"></i>',
        previous: '<i class="fa fa-chevron-left"></i> Anterior'
    }
    , onStepChanging: function (event, currentIndex, newIndex) {
         //console.log('Current Index:' + currentIndex + " New Index:" + newIndex);
        //return currentIndex > newIndex || !(3 === newIndex && Number($("#age-2").val()) < 18) && (currentIndex < newIndex && (form.find(".body:eq(" + newIndex + ") label.error").remove(), form.find(".body:eq(" + newIndex + ") .error").removeClass("error")), form.validate().settings.ignore = ":disabled,:hidden", form.valid())
        //return (currentIndex < newIndex && (form.find(".body:eq(" + newIndex + ") label.error").remove(), form.find(".body:eq(" + newIndex + ") .error").removeClass("error")), form.validate().settings.ignore = ":disabled,:hidden", form.valid());
        if (0 === currentIndex && $("#CustomerIdAsociarRecibo").val() === '') {
            //form.valid();
            $("#alertamsj").css('display', 'block');
            $("#alertamsj").text('Campos requeridos!');
            return false;
        }
        else if (1 === currentIndex && $("#ddlRecibosAsociados").children('option').length === 0 && (currentIndex < newIndex) )
        {
            $("#alertamsj").css('display', 'block');
            $("#alertamsj").text('Debe agregar certificados si desea continuar!');
            return false;
        }
        else {
            //form.find(".body:eq(" + newIndex + ") .error").removeClass("error");
            $("#alertamsj").css('display', 'none');
        }

        return (0 === currentIndex && $("#CustomerIdAsociarRecibo").val() !== '') || (currentIndex > newIndex) || (newIndex ===2)
    }
    , onFinishing: function (event, currentIndex) {
        return form.validate().settings.ignore = ":disabled", form.valid();

    }
    , onFinished: function (event, currentIndex) {
        var form = $(this);
        form.submit();
       //  swal("Form Submitted!", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed lorem erat eleifend ex semper, lobortis purus sed.");
    }
}), $(".validation-wizard").validate({
    ignore: "input[type=hidden]"
    , errorClass: "text-danger"
    , successClass: "text-success"
    , highlight: function (element, errorClass) {
        $(element).removeClass(errorClass)
    }
    , unhighlight: function (element, errorClass) {
        $(element).removeClass(errorClass)
    }
    , errorPlacement: function (error, element) {
        error.insertAfter(element)
    }
    , rules: {
        email: {
            email: !0
        }
    }
})