Common = {
    Alert: function (message) {
        $.notify(message, { color: "#fff", background: "#A5881B", align: "top", verticalAlign: "right" });
    },

    Error: function (message) {
        $.notify(message, { color: "#fff", background: "#D44950", align: "top", verticalAlign: "right" });
    },

    Success: function (message) {
        $.notify(message, { color: "#fff", background: "#20D67B", align: "top", verticalAlign: "right" });
    },

    Notify: function (message) {
        $.notify(message, { color: "#fff", background: "#4B7EE0", align: "top", verticalAlign: "right" });
    },

    ShowLoader: function () {
        // if the element doesn't exist, it will create a one new with the predefined html structure and css
        var lb = new $.LoadingBox({

            // if the element doesn't exist, it will create a one new with the predefined html structure and css
            mainElementID: 'loading-box',

            // animation speed
            fadeInSpeed: 'normal',
            fadeOutSpeed: 'normal',

            // opacity
            opacity: 0.3,

            // background color
            backgroundColor: "#000",

            // width / height of the loading GIF
            loadingImageWitdth: "60px",
            loadingImageHeigth: "60px",

            // path to the loading gif
            loadingImageSrc: "/images/load1.gif"
            
        }); 
       
    },

    HideLoader: function () {
        $('#loading-box').fadeOut('normal');
    }
}