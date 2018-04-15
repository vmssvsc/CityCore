Slider = {
    Variables: {
        srcUpload: '/Slider/Save',
        srcDelete: '/Slider/Delete'
    },
    Controls: {
        btnSave: '.btnUpload',
        sliderImageInput: '.sliderImage',
        btnPreview: '.btnPreveiew',
        btnDelete: '.btnDelete',
        seqNo: '.seqNo',
        preview: '.preview',
        fileInput: '.sliderImage',
        sliderName: '.sliderName'
    }

};

$(document).ready(function () {
    $(Slider.Controls.btnSave).click(function () {
        var seqNum = $(this).attr('data-seq');
        var fdata = new FormData();
        var fileInput = $('input[data-seq="' + seqNum +'"]')[0];
        var file = fileInput.files[0];
        fdata.append("sequenceNo", seqNum);
        fdata.append("file", file);
        $.ajax({
            type: 'post',
            url: Slider.Variables.srcUpload,
            data: fdata,
            processData: false,
            contentType: false,
            success: function (data) {
                if (data.success) {
                    //Set controls
                    $(Slider.Controls.btnDelete + '[data-seq="' + seqNum + '"]').show();
                    $(Slider.Controls.sliderName + '[data-seq="' + seqNum + '"]').show();
                    $(Slider.Controls.fileInput + '[data-seq="' + seqNum +'"]').hide();
                    $(Slider.Controls.btnSave + '[data-seq="' + seqNum +'"]').hide();
                    
                    $(Slider.Controls.btnDelete + '[data-seq="' + seqNum + '"]').attr("data-id", data.id);
                    $(Slider.Controls.preview + '[data-seq="' + seqNum + '"]').attr("data-pic", data.url);
                    $(Slider.Controls.preview + '[data-seq="' + seqNum + '"]').attr("data-pic-title", data.name);
                    $(Slider.Controls.sliderName + '[data-seq="' + seqNum + '"]').html(data.name);
                    $(Slider.Controls.preview + '[data-seq="' + seqNum + '"]').show();
                    
                    Common.Success(data.message);
                    $(() => $.imgPreview());
                }
                else {
                    Common.Error(data.message);
                }
            },
            error: function (xhr, status, errorThrown) {
                Common.Error(errorThrown);
            }
        });
    });
    $(() => $.imgPreview());

    $(Slider.Controls.btnDelete).click(function () {
        var seq = $(this).attr('data-seq');
        bootbox.confirm("Are you sure you want to delete this Slider?",
            function (result) {
                if (result) {
                    $.ajax({
                        type: 'GET',
                        url: Slider.Variables.srcDelete + "?sequenceNumber=" + seq,
                        success: function (data) {
                            if (data.success) {
                                Common.Success(data.message);
                                window.location.reload();
                            }
                            else {
                                Common.Error(data.message);
                            }
                        },
                        error: function (xhr, status, errorThrown) {
                            Common.Error(errorThrown);
                        }
                    });
                }
                
            });
        
    });
});