/**
 * @desc Image preview plugin
 * @author haibao[http://www.hehaibao.com/]
 */
;($ => {
    $.imgPreview = options => new imgPreview(options);
    class imgPreview {
        constructor(options) {
            this.init(options);
        }
        init(options) {
           //default allocation
            this.config = $.extend(true, {}, {
                el: '[data-pic]', // elements that need to be previewed
                attr: 'data-pic', //The original image address to be previewed
                attrTitle: 'data-pic-title', //image title
                attrDesc: 'data-pic-desc', // picture description
                mode: 'single', //preview mode, default - single: single mode; multiple: 
                isMaskShow: true, //whether to show the mask layer
                maskBgColor: 'rgba(0,0,0,.5)' ,// mask layer background color default black transparency 50%
           
                
            }, options);
            
           //Core Business Click on image preview
            this.allDom().each((i ,item) => {
                const _self = this;
                $(item).off('click').on('click', (e) => _self.preview(item));
            });
        }
        preview(item) {
            const _self = this;
            const [$win, $body, $item] = [$(window), $('body'), $(item)];
            const [screenW, screenH] = [$win.width(), $win.height()];
            
            // Get the data cached in the custom property, ie: large map address, title, description
            const [picUrl, picTitle, picDesc] = [$item.attr(_self.config.attr), $item.attr(_self.config.attrTitle), $item.attr(_self.config.attrDesc)];
            
            //Set the initial position of the window)
            const [initX, initY] = [event.clientX, event.clientY];

            //Create preview mask
            _self.imgPreviewMask = $(`<div class="img-preview-mask" style="background-color: ${_self.config.maskBgColor || 'rgba(0,0,0,.5)'}"></div>`);
      //Create popup preview window
            _self.imgPreviewPopover = $(`<div class="img-preview-popover" style="top: ${initY}px;left: ${initX}px;"></div>`);
          //Create popup preview window Content box
            _self.imgPreviewBox = $('<div class="img-preview-box"></div>');
          //Create pop-up preview window bottom title, description box
            _self.imgPreviewFoot = $('<div class="img-preview-foot"></div>');

           // (Create a preview)
            if(_self.config.mode === 'single') {
                //( Single graph mode)
                let img = new Image();
                let createPic = ((w, h) => {
                  // Prevent the picture from exceeding the height of the screen, distinguish between mobile phone and PC
                    if(_self.isPc()) {
                        h = parseInt((screenW * h) / w);
                    } else {
                        h = h >= screenH ? screenH : h;
                    }
                    w = w >= screenW ? screenW : w;

                    let styleArr = {
                        width: w,
                        height: _self.isPc() ? 'auto' : h,
                        top: parseInt((screenH - h) / 2),
                        left: parseInt((screenW - w ) / 2)
                    }

                    _self.imgPreviewBox.append($(`<img src="${picUrl}" height="100%" width="100%"/>`))
                        .click(() => {
                            _self.hide();
                        });

             //( Verify if a mask layer is needed)
                    if(_self.config.isMaskShow) {
                        //( Create a mask layer)
                        $body.append(_self.imgPreviewMask);
                        //(Click on mask off)
                        _self.imgPreviewMask.click(() => {
                            _self.hide();
                        });
                    }

                    // (Verify if the header is passed in)
                    if(_self.config.attrTitle) {
                        //(Create a title)
                        _self.imgPreviewFoot.append($(`<div class="img-foot-title">${picTitle || ''}</div>`));
                    }

                    //( Verify whether the description is passed in)
                    if(_self.config.attrDesc) {
                        //(Create a description)
                        _self.imgPreviewFoot.append($(`<div class="img-foot-desc">${picDesc || ''}</div>`));
                    }

                   //(Add content)
                    _self.imgPreviewPopover.append(_self.imgPreviewBox);
                    if(picTitle || picDesc) {
                        _self.imgPreviewPopover.append(_self.imgPreviewFoot);
                    }
                    $body.append(_self.imgPreviewPopover);

                    // Execute animation compatible with zepto does not support animate
                    if($.fn && $.fn.jquery) {
                        _self.imgPreviewPopover.animate(styleArr, 300);
                    } else {
                        _self.imgPreviewPopover.css(styleArr);
                    }
                });
                img.onload = (() => {
                    //( Call the callback function asynchronously when the image download is complete)
                    img.onload = null;
                    createPic(img.width, img.height);
                });
                img.onerror = (() => {
                    //(The image failed to load)
                        alert('Sorry, the image failed to load！');
                    return;
                });
                img.src = picUrl;
            }
        }
        isPc() {
            //(Whether the mobile phone side)
            return /Android|webOS|iPhone|iPod|BlackBerry/i.test(navigator.userAgent);
        }
        hide() {
            //(Close the preview)
            this.imgPreviewMask.remove();
            this.imgPreviewPopover.remove();
        }
        allDom() {
            //( Get all the elements)
            return $(this.config.el);
        }
    }
})(window.Zepto || window.jQuery);