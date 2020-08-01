'use strict';
// fixed svg show
//-----------------------------------------------------------------------------
svg4everybody();

// checking if element for page
//-----------------------------------------------------------------------------------
function isOnPage(selector) {
    return ($(selector).length) ? $(selector) : false;
}
// search page
function pageWidget(pages) {
  var widgetWrap = $('<div class="widget_wrap"><ul class="widget_list"></ul></div>');
  widgetWrap.prependTo("body");
  for (var i = 0; i < pages.length; i++) {
    if (pages[i][0] === '#') {
      $('<li class="widget_item"><a class="widget_link" href="' + pages[i] +'">' + pages[i] + '</a></li>').appendTo('.widget_list');
    } else {
      $('<li class="widget_item"><a class="widget_link" href="' + pages[i] + '.html' + '">' + pages[i] + '</a></li>').appendTo('.widget_list');
    }
  }
  var widgetStilization = $('<style>body {position:relative} .widget_wrap{position:fixed;top:0;left:0;z-index:9999;padding:20px 20px;background:#222;border-bottom-right-radius:10px;-webkit-transition:all .3s ease;transition:all .3s ease;-webkit-transform:translate(-100%,0);-ms-transform:translate(-100%,0);transform:translate(-100%,0)}.widget_wrap:after{content:" ";position:absolute;top:0;left:100%;width:24px;height:24px;background:#222 url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQAgMAAABinRfyAAAABGdBTUEAALGPC/xhBQAAAAxQTFRF////////AAAA////BQBkwgAAAAN0Uk5TxMMAjAd+zwAAACNJREFUCNdjqP///y/DfyBg+LVq1Xoo8W8/CkFYAmwA0Kg/AFcANT5fe7l4AAAAAElFTkSuQmCC) no-repeat 50% 50%;cursor:pointer}.widget_wrap:hover{-webkit-transform:translate(0,0);-ms-transform:translate(0,0);transform:translate(0,0)}.widget_item{padding:0 0 10px}.widget_link{color:#fff;text-decoration:none;font-size:15px;}.widget_link:hover{text-decoration:underline} </style>');
  widgetStilization.prependTo(".widget_wrap");
}

$(document).ready(function($) {
  //pageWidget(['index', 'referral', 'privacy-policy', 'certificates', 'faq', 'price', 'detail', 'requisites']);
});

function matchColumns(){
  $('.column').removeAttr('style');
  $('.mod-img').each(function(){
    var highestBox = 0;
    $('.column', this).each(function(){
      if($(this).outerHeight() > highestBox) {
        highestBox = $(this).outerHeight();
      }
    });
    $('.column',this).outerHeight(highestBox);
  });
}

$(document).ready(function () {
  matchColumns();
});

$(window).on("load debounce", function() {
  matchColumns();
});

if (document.addEventListener) {
  document.addEventListener("mousewheel", MouseWheelHandler(), false);
  document.addEventListener("DOMMouseScroll", MouseWheelHandler(), false);
} else {
  sq.attachEvent("onmousewheel", MouseWheelHandler());
}
function MouseWheelHandler() {
  return function (e) {
    var e = window.event || e;
    var delta = Math.max(-1, Math.min(1, (e.wheelDelta || -e.detail)));
      if (delta > 0) {
        if ($(window).scrollTop() <= 0){
          $('#header-outer').removeClass('detached');
        }
        $('#header-outer').removeClass('invisible');
      }
      else {
        if ($(window).scrollTop() >= $('#header').innerHeight()){
          $('#header-outer').addClass('invisible');
        }
        if ($(window).scrollTop() >= 100){
          $('#header-outer').addClass('detached');
        }
      }
      return false;
  }
}
$(document).on('click', '.js-menu', function (e) {
    e.preventDefault();
    $('.header').toggleClass('openmenu');
});

$('.popup').miniTip();

$(document).on('click', '.faq-item', function (e) {
    e.preventDefault();
    $(this).toggleClass('active');
});

$('[data-remodal-id=modal]').remodal();
var each_bar_width = 0,
    countAnim = true;
function startProcess() {
  var each_bar_width = 0;
  $(".line-content").each(function(){
    each_bar_width = $(this).attr('aria-valuenow');
    $(this).css('width', each_bar_width + '%');
  });
  if (countAnim) {
    $('.progress-number').each(function() {
      var _this = $(this);
      if (_this.attr('data-percentage') != '100%'){
        _this.animate({'opacity': '1'}, 200, function () {
          _this.animate({
            left: _this.attr('data-percentage')
          }, {
            duration: 900,
            step: function(now) {
              var data = Math.round(now);
              _this.html(_this.attr('data-percentage'));
            }
          });
        });

      } else {
        _this.animate({'opacity': '1'}, 200, function () {
          _this.animate({
            left: _this.attr('data-percentage')
          }, {
            duration: 900,
            step: function(now) {
              var data = Math.round(now);
              _this.html(data + '%');
            }
          });
        });

      }

    });
    countAnim = false;
  }

}


$( window ).scroll(function() {
  if ($('#process').length) {
    if($( window ).scrollTop() > $('#process').offset().top - 600){
      startProcess();
    }
  }
});
// custom jQuery validation
//-----------------------------------------------------------------------------------
var validator = {
    init: function() {
        $('form').each(function() {
            var name = $(this).attr('name');
            if (valitatorRules.hasOwnProperty(name)) {
                var rules = valitatorRules[name];
                $(this).validate({
                    rules: rules,
                    errorElement: 'b',
                    errorClass: 'error',
                    focusInvalid: false,
                    focusCleanup: false,
                    onfocusout: function(element) {
                        var $el = validator.defineElement($(element));
                        $el.valid();
                    },
                    errorPlacement: function(error, element) {
                        validator.setError($(element), error);
                    },
                    highlight: function(element, errorClass, validClass) {
                        var $el = validator.defineElement($(element)),
                            $elWrap = $el.closest('.el-field');
                        if ($el) {
                            $el.removeClass(validClass).addClass(errorClass);
                            $elWrap.removeClass('show-check');
                            if ($el.closest('.ui.dropdown').length) {
                                $el.closest('.ui.dropdown').addClass('error');
                            }
                        }
                    },
                    unhighlight: function(element, errorClass, validClass) {
                        var $el = validator.defineElement($(element)),
                            $elWrap = $el.closest('.el-field');
                        if ($el) {
                            $el.removeClass(errorClass).addClass(validClass);
                            if ($elWrap.hasClass('check-valid')) {
                                $elWrap.addClass('show-check');
                            }
                            $el.closest('el-field').addClass('show-check');
                            if ($el.val() == '') {
                                $el.removeClass('valid');
                            }
                            if ($el.closest('.ui.dropdown').length) {
                                $el.closest('.ui.dropdown').removeClass('error');
                            }
                        }
                    },
                    messages: {
                        'user_email': {
                            required: 'Поле обязательное',
                            email: 'Неправильный формат E-mail'
                        },
                        'user_name': {
                            required: 'Поле обязательное',
                            letters: 'Неправильный формат имени',
                            minlength: 'Не меньше 2 символов'
                        },
                        'user_login': {
                            required: 'Поле обязательное',
                            email: 'Неправильный формат E-mail'
                        },
                        'user_password': {
                            required: 'Поле обязательное',
                            minlength: 'Не менее 6-ти символов'
                        },
                        'user_password_confirm': {
                            required: 'Вы не подтвердили пароль',
                            minlength: 'Не менее 6-ти символов',
                            equalTo: 'Пароли должны совпадать'
                        },
                        'user_phone': {
                            required: 'Поле обязательное',
                            digits: 'Неправильный формат номера'
                        }
                    }
                });
            }
        });
    },
    setError: function($el, message) {
        $el = this.defineElement($el);
        if ($el) this.domWorker.error($el, message);
    },
    defineElement: function($el) {
        return $el;
    },
    domWorker: {
        error: function($el, message) {
            if ($el.attr('type') == 'file') $el.parent().addClass('file-error');
            $el.addClass('error');
            $el.after(message);
        }
    }
};


// rule for form namespace
//-----------------------------------------------------------------------------------
var valitatorRules = {
    'form_one': {
        'user_login': {
            required: true,
            email: true
        },
        'user_name': {
            required: true,
            minlength: 2
        },
        'user_email': {
            required: true,
            email: true
        },
        'user_phone': {
            required: true,
            digits: true
        },
        'user_password': {
            required: true,
            minlength: 6
        },
        'user_password_confirm': {
            required: true,
            minlength: 6,
            equalTo: "#user_password"
        }
    }

};

// custom rules
//-----------------------------------------------------------------------------------
$.validator.addMethod("email", function(value) {
    if (value == '') return true;
    var regexp = /[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/;
    return regexp.test(value);
});

$.validator.addMethod("letters", function(value, element) {
    return this.optional(element) || /^[^1-9!@#\$%\^&\*\(\)\[\]:;,.?=+_<>`~\\\/"]+$/i.test(value);
});

$.validator.addMethod("digits", function(value, element) {
    return this.optional(element) || /^(\+?\d+)?\s*(\(\d+\))?[\s-]*([\d-]*)$/i.test(value);
});

$.validator.addMethod("valueNotEquals", function(value, element) {
    if (value == "") return false
    else return true
});

//  validator init
//-----------------------------------------------------------------------------------
validator.init();

if (isOnPage($('#map__network'))) {
// підключаю richmarker
//-----------------------------------------------------------------------------------

  // var script = '<script type="text/javascript" src="https://raw.githubusercontent.com/googlemaps/js-rich-marker/gh-pages/src/richmarker.js" ></script>';
  // document.write(script);

  function init() {
    var latlng = new google.maps.LatLng(48.4764386,135.0536667);
    var myOptions = {
      zoom: 15,
      center: latlng,
      zoomControl: false,
      disableDoubleClickZoom: false,
      mapTypeControl: false,
      scaleControl: false,
      scrollwheel: false,
      panControl: false,
      streetViewControl: false,
      draggable : true,
      overviewMapControl: false,
      overviewMapControlOptions: {
        opened: false
      },
      styles: [
        {
          "featureType": "administrative",
          "elementType": "labels.text.fill",
          "stylers": [
            {
              "color": "#444444"
            }
          ]
        },
        {
          "featureType": "landscape",
          "elementType": "all",
          "stylers": [
            {
              "color": "#f2f2f2"
            }
          ]
        },
        {
          "featureType": "poi",
          "elementType": "all",
          "stylers": [
            {
              "visibility": "off"
            }
          ]
        },
        {
          "featureType": "poi.business",
          "elementType": "geometry.fill",
          "stylers": [
            {
              "visibility": "on"
            }
          ]
        },
        {
          "featureType": "road",
          "elementType": "all",
          "stylers": [
            {
              "saturation": -100
            },
            {
              "lightness": 45
            }
          ]
        },
        {
          "featureType": "road.highway",
          "elementType": "all",
          "stylers": [
            {
              "visibility": "simplified"
            }
          ]
        },
        {
          "featureType": "road.arterial",
          "elementType": "labels.icon",
          "stylers": [
            {
              "visibility": "off"
            }
          ]
        },
        {
          "featureType": "transit",
          "elementType": "all",
          "stylers": [
            {
              "visibility": "off"
            }
          ]
        },
        {
          "featureType": "water",
          "elementType": "all",
          "stylers": [
            {
              "color": "#b4d4e1"
            },
            {
              "visibility": "on"
            }
          ]
        }
      ]

    };
    var map = new google.maps.Map(document.getElementById("map__network"),
      myOptions);

    function clickroute(lati, long) {
      var latLng = new google.maps.LatLng(lati, long);
      map.panTo(latLng);
    }
    var data = [
      {
        "index": 0,
        "latitude": 48.4764386,
        "longitude": 135.0536667,
        "url": "#item-9",
        "status": "error",
        "title": "$ 200 000"
      }
    ];
    for (var i = 0; i < data.length; i++) {

      var marker = new RichMarker({
        position: new google.maps.LatLng(data[i].latitude, data[i].longitude),
        map: map,
        height: 38,
        shadow: false,
        content: '<div class="animated-dot"><div class="middle-dot"></div><div class="signal"></div><div class="signal2"></div></div>'
      });
    }
    $(document).on('click', '.network__item .local', function (e) {
      e.preventDefault();
      var lati = $(this).attr('data-lati'),
        long = $(this).attr('data-long');
      $('.network__item').removeClass('active');
      $(this).parents('.network__item').addClass('active');
      clickroute(lati, long);
    });
  }

  google.maps.event.addDomListener(window, 'load', init);

  $(document).on('click', '.marker-custom', function (e) {
    e.preventDefault();
    var $wrap = $('.js-height-wrap'),
      $this = $(this);

    $('.marker-custom').removeClass('active');
    $wrap.find('.network__item').removeClass('active');

    $this.addClass('active');
    $wrap.mCustomScrollbar('scrollTo', $this.attr('href'));
    $wrap.find($this.attr('href')).addClass('active');
  });

}

"use strict";

$.fn.youtubeVideo = function (player) {

    var $wrapper = $(this),
        $videoWrap = $(".video-youtube-wrap", this),
        $videoId = $videoWrap.attr('data-id-video'),
        $play = $(".btn-play", $wrapper),
        $pause = $(".btn-pause", $wrapper),
        $playCenter = $(".center-btm-play-youtube", $wrapper),
        $btnMute = $(".btn-mute", $wrapper),
        $current = $(".video-current", $wrapper),
        $duration = $(".video-duration", $wrapper),
        $scrubber = $(".video-scrubber", $wrapper),
        $progressVideo = $(".video-progress", $wrapper),
        $fullScreenBtn = $('.full-screen-youtube', $wrapper),
        $volume_wrapper = $(".volume", $wrapper),
        $volume_bar = $(".volume-bar", $wrapper),
        time_update_interval = 0,
        $position = true,
        $percentage = true;


    $videoWrap.css('background-image', 'url(https://img.youtube.com/vi/' + $videoId + '/maxresdefault.jpg)');
    // $videoItem.css('background-image', 'url(https://img.youtube.com/vi/' + videoId + '/maxresdefault.jpg)');
    // $videoItem.css('background-image', 'url(https://img.youtube.com/vi/'+videoId+'/sddefault.jpg)');


    initialize()
    function initialize() {

        updateTimerDisplay();
        updateVolume(0, 0.52);
        clearInterval(time_update_interval);

        time_update_interval = setInterval(function () {
            updateTimerDisplay();
        }, 1000);

    }

// # TODO: Scrubber/Progress Bar
    // # -----------------------------------------------------------------------------------
    var timeDrag = false;

    $scrubber.on("mousedown", function (e) {
        timeDrag = true;
        updateScrubber(e.pageX);
    });

    $(document).on("mouseup", function (e) {
        if (timeDrag) {
            timeDrag = false;
            updateScrubber(e.pageX);
        }
    });

    $(document).on("mousemove", function (e) {
        if (timeDrag) {
            updateScrubber(e.pageX);
        }
    });

    function updateScrubber(x) {

        var maxduration = player.getDuration(),
            position = x - $scrubber.offset().left,
            percent = 100 * position / $scrubber.width();

        if (percent > 100) {
            percent = 100;
        }
        if (percent < 0) {
            percent = 0;
        }
        $progressVideo.width((percent) + "%");

        player.seekTo(maxduration * percent / 100);
    };



// # TODO: Sound volume
// # -----------------------------------------------------------------------------------
    $btnMute.on('click', function () {
        var mute_toggle = $(this);

        if (player.isMuted()) {
            player.unMute();
            mute_toggle.removeClass('muted');
            $volume_bar.css("width", player.getVolume() + "%");
        }
        else {
            player.mute();
            mute_toggle.addClass('muted');
            $volume_bar.css("width", 0);
        }
    });


    var volumeDrag = false;
    $volume_wrapper.on("mousedown", function (e) {
        volumeDrag = true;
        updateVolume(e.pageX);
    });

    $(document).on("mouseup", function (e) {
        if (volumeDrag) {
            volumeDrag = false;
            updateVolume(e.pageX);
        }
    });

    $(document).on("mousemove", function (e) {
        if (volumeDrag) {
            updateVolume(e.pageX);
        }
    });

    function updateVolume(x, vol) {

        if (vol) {
            $percentage = vol * 100;
        } else {
            $position = x - $volume_wrapper.offset().left;
            $percentage = 100 * $position / $volume_wrapper.width();
        }

        if ($percentage > 100) {
            $percentage = 100;
        }
        if ($percentage < 0) {
            $percentage = 0;
        }

        $volume_bar.css("width", $percentage + "%");
        player.setVolume($percentage / 1);


        if ($percentage / 1 == 0) {
            $btnMute.addClass("muted");
        } else {
            $btnMute.removeClass("muted");
        }

    }

// # TODO: Play/Pause
// # -----------------------------------------------------------------------------------
    $play.on('click', function () {
        player.playVideo();
        $wrapper.addClass('start play');
    });

    $playCenter.on('click', function () {
        player.playVideo();
        $wrapper.addClass('start play');
    });

    $pause.on('click', function () {
        player.pauseVideo();
        $wrapper.removeClass('play');
    });


// # TODO: Update current time text display.
// # -----------------------------------------------------------------------------------
    function updateTimerDisplay() {
        $current.text(formatTime(player.getCurrentTime()));
        $duration.text(formatTime(player.getDuration()));

        $progressVideo.width((player.getCurrentTime() / player.getDuration() * 100) + "%");

        if (player.getCurrentTime() == player.getDuration()) {
            $wrapper.removeClass("play");
        }
    }

// # TODO: Helper Functions
// # -----------------------------------------------------------------------------------
    function formatTime(time) {
        time = Math.round(time);

        var minutes = Math.floor(time / 60),
            seconds = time - minutes * 60;

        seconds = seconds < 10 ? '0' + seconds : seconds;

        return minutes + ":" + seconds;
    }

// # TODO: Full Screen Button
// # -----------------------------------------------------------------------------------
    $fullScreenBtn.on('click', function () {
        playFullscreen();
    });

    if (document.addEventListener) {
        document.addEventListener('webkitfullscreenchange', exitHandler, false);
        document.addEventListener('mozfullscreenchange', exitHandler, false);
        document.addEventListener('fullscreenchange', exitHandler, false);
        document.addEventListener('MSFullscreenChange', exitHandler, false);
    }
    function exitHandler() {
        if (document.webkitIsFullScreen || document.mozFullScreen || document.msFullscreenElement !== null) {
            if ((document.webkitIsFullScreen == false) || (document.mozFullScreen == false) || (document.msFullscreenElement == false)){
                $('body').removeClass('full');
            }
        }
    }
    function playFullscreen() {

        var docElm = $wrapper[0];
        var isInFullScreen = (document.fullscreenElement && document.fullscreenElement !== null) ||
            (document.webkitFullscreenElement && document.webkitFullscreenElement !== null) ||
            (document.mozFullScreenElement && document.mozFullScreenElement !== null) ||
            (document.msFullscreenElement && document.msFullscreenElement !== null);

        if (!isInFullScreen) {
            $('body').addClass('full');
            if (docElm.requestFullscreen) {
                docElm.requestFullscreen();
            } else if (docElm.mozRequestFullScreen) {
                docElm.mozRequestFullScreen();
            } else if (docElm.webkitRequestFullScreen) {
                docElm.webkitRequestFullScreen();
            } else if (docElm.msRequestFullscreen) {
                docElm.msRequestFullscreen();
            }
        } else {
            $('body').removeClass('full');
            if (document.exitFullscreen) {
                document.exitFullscreen();
            } else if (document.webkitExitFullscreen) {
                document.webkitExitFullscreen();
            } else if (document.mozCancelFullScreen) {
                document.mozCancelFullScreen();
            } else if (document.msExitFullscreen) {
                document.msExitFullscreen();
            }
        }

    }

}



var tag = document.createElement('script');
tag.src = "https://www.youtube.com/iframe_api";
var firstScriptTag = document.getElementsByTagName('script')[0];
firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

function onYouTubeIframeAPIReady() {
    $(".video-player-youtube").each(function () {
        var playerid = $(this).find('.video-placeholder').attr('id'),
            videoid = $(this).find('.video-youtube-wrap').attr('data-id-video');

        var curplayer = createPlayer(playerid, videoid);
    });
}

function createPlayer(playerID, videoID) {
    return new YT.Player(playerID, {
        width: 600,
        height: 400,
        videoId: videoID,
        playerVars: {
            'modestbranding': 1,
            'rel': 0,
            'controls': 0,
            'showinfo': 0,
            'autoplay': 0,
            'playsinline': 1,
            'loop': 1,
            'color': 'red',
            'autohide': 0,
            'iv_load_policy': 3,
            'nologo': 1,
            'fs': 0
        },
        events: {
            onReady: onPlayerReady
        }
    })
    
}

function onPlayerReady(event) {
    var player = event.target,
        $wraper = $('#'+player.a.id).closest('.video-player-youtube'),
        controlsHtml = '<button class="center-btm-play-youtube"></button><div class="video-controls"><div class="video-scrubber"><div class="video-progress"></div></div><button id="play" class="icon-play btn-play"></button><button id="pause" class="icon-pause btn-pause"></button><button id="mute-toggle" class="icon-mute btn-mute"></button><div class="volume" title="Set volume"><span class="volume-bar"></span></div><div class="video-time"><span id="current-time" class="video-current">0:00</span> / <span id="duration" class="video-duration">0:00</span></div><button class="full-screen-youtube"></button></div>';

    $wraper.append(controlsHtml);
    $wraper.youtubeVideo(player);
}



















$(function() {

  var $container = $('.js-index');
  // $container.find('');
  function preloadImages(urls, allImagesLoadedCallback) {
    var loadedCounter = 0,
      toBeLoadedNumber = urls.length;

    urls.forEach( function(url) {
      preloadImage(url, function() {
        // console.log(url);
        loadedCounter++;
        if( loadedCounter == toBeLoadedNumber ) {
          allImagesLoadedCallback();
        }
      });
    });

    function preloadImage(url, anImageLoadedCallback) {
      var img = new Image();
      img.src = url;
      img.onload = anImageLoadedCallback;
    }
  }
  if (isOnPage($container)){
    preloadImages([
      '/Content/html3/assets/img/bg-body-1.jpg',
      '/Content/html3/assets/img/bg-body-2.jpg',
      '/Content/html3/assets/img/bg-body-3.jpg',
      '/Content/html3/assets/img/bg-body-4.jpg',
      '/Content/html3/assets/img/bg-body-5.jpg',
      '/Content/html3/assets/img/bg-body-6.jpg',
      '/Content/html3/assets/img/bg-body-7.jpg',
      '/Content/html3/assets/img/bg-body-8.jpg',
      '/Content/html3/assets/img/bg-body-9.jpg',
      '/Content/html3/assets/img/bg-body-10.jpg',
      '/Content/html3/assets/img/bg-body-11.jpg',
      '/Content/html3/assets/img/bg-body-12.jpg',
      '/Content/html3/assets/img/bg-body-13.jpg',
      '/Content/html3/assets/img/bg-body-14.jpg',
      '/Content/html3/assets/img/bg-body-15.jpg',
      '/Content/html3/assets/img/bg-body-16.jpg',
      '/Content/html3/assets/img/bg-body-17.jpg',
      '/Content/html3/assets/img/bg-body-18.jpg',
      '/Content/html3/assets/img/bg-body-19.jpg',
      '/Content/html3/assets/img/bg-body-20.jpg',
      '/Content/html3/assets/img/bg-body-back-1.jpg',
      '/Content/html3/assets/img/bg-body-back-2.jpg',
      '/Content/html3/assets/img/bg-body-back-3.jpg',
      '/Content/html3/assets/img/bg-body-back-4.jpg',
      '/Content/html3/assets/img/bg-body-back-5.jpg',
      '/Content/html3/assets/img/bg-body-back-6.jpg',
      '/Content/html3/assets/img/bg-body-back-7.jpg',
      '/Content/html3/assets/img/bg-body-back-8.jpg',
      '/Content/html3/assets/img/bg-body-back-9.jpg',
      '/Content/html3/assets/img/bg-body-back-10.jpg',
      '/Content/html3/assets/img/bg-body-back-11.jpg',
      '/Content/html3/assets/img/bg-body-back-12.jpg',
      '/Content/html3/assets/img/bg-body-back-13.jpg',
      '/Content/html3/assets/img/bg-head-1.jpg',
      '/Content/html3/assets/img/bg-head-2.jpg',
      '/Content/html3/assets/img/bg-head-3.jpg',
      '/Content/html3/assets/img/bg-head-4.jpg',
      '/Content/html3/assets/img/bg-head-5.jpg',
      '/Content/html3/assets/img/bg-head-6.jpg',
      '/Content/html3/assets/img/bg-head-7.jpg',
      '/Content/html3/assets/img/bg-head-8.jpg',
      '/Content/html3/assets/img/bg-head-9.jpg',
      '/Content/html3/assets/img/bg-head-10.jpg',
      '/Content/html3/assets/img/bg-head-11.jpg',
      '/Content/html3/assets/img/bg-4-1.jpg'
    ], function(){
      console.log('complete');
    });
  }

  var $slider = $container.find('.main-carousel');

  $slider.slick({
    dots: false,
    infinite: true,
    // draggable: false,
    speed: 500,
    prevArrow: '<button type="button" class="slick-prev slick-arrow"><svg class="icon icon-arrow-l"><use xlink:href="assets/img/symbol/sprite.svg#arrow-l"></use></svg><div class="slide-count"><span class="slide-current">1</span><i class="line"></i><span class="slide-total">5</span></div></button>',
    nextArrow: '<button type="button" class="slick-next slick-arrow"><div class="slide-count"><span class="slide-current">1</span><i class="line"></i><span class="slide-total">5</span></div><svg class="icon icon-arrow-r"><use xlink:href="assets/img/symbol/sprite.svg#arrow-r"></use></svg></button>'
  });
  if ($('.slick-arrow').length) {
    $('.slick-arrow .slide-total').text($slider.slick('getSlick').$slides.length);
  }

  $slider.on('afterChange', function(event, slick, currentSlide){
    $('.slick-arrow .slide-current').text(parseInt(currentSlide+1));
    // wow.init();
      $('.wow').removeClass('animated');
      $('.wow').removeAttr('style');
      new WOW({
        boxClass:     'slick-active .wow',
        animateClass: 'animated',
        offset:       0,
        mobile:       true,
        live:         true,
        callback:     function(box) {

        },
        scrollContainer: null,
        resetAnimation: true
      }).init();
  });
  var wow = new WOW(
    {
      boxClass:     'slick-active .wow',
      animateClass: 'animated',
      offset:       0,
      mobile:       true,
      live:         true,
      callback:     function(box) {

      },
      scrollContainer: null,
      resetAnimation: true
    }
  );
  wow.init();


  var anim = new WOW(
    {
      boxClass:     'anim',
      animateClass: 'animated',
      offset:       100,
      mobile:       true,
      live:         true,
      callback:     function(box) {

      },
      scrollContainer: null,
      resetAnimation: true
    }
  );
  anim.init();
  var atImg;
  $('.js-link-area1').mouseenter(function () {
    atImg = $(this).attr('data-img');
    $('.head-img').attr('src', atImg);
  });

  $('.js-link-area1').mouseleave(function () {
    atImg = $(this).attr('data-default');
    $('.head-img').attr('src', atImg);
  });

  $('.js-link-area2').mouseenter(function () {
    atImg = $(this).attr('data-img');
    $('.body-img').attr('src', atImg);
  });

  $('.js-link-area2').mouseleave(function () {
    atImg = $(this).attr('data-default');
    $('.body-img').attr('src', atImg);
  });

  $('.js-link-area3').mouseenter(function () {
    atImg = $(this).attr('data-img');
    $('.body-img').attr('src', atImg);
  });

  $('.js-link-area3').mouseleave(function () {
    atImg = $(this).attr('data-default');
    $('.body-img').attr('src', atImg);
  });


  $('.body-wrap map area').mouseenter(function () {
    var atImg = $(this).attr('data-img');
    $('.body-wrap .body-img').attr('src', atImg);
    var dataName = $(this).attr('data-href');
    $(dataName).siblings().removeClass('active');
    $(dataName).addClass('active');
  }).click(function () {
    var dataName = $(this).attr('data-href');
    window.location.href = $(dataName).find("a").eq(0).attr("href"); return false;
  });
  $('.body-wrap map area').mouseleave(function () {
    var atImg = $(this).attr('data-default');
    $('.body-wrap .body-img').attr('src', atImg);
    var dataName = $(this).attr('data-href');
    $(dataName).removeClass('active');
  });

  $('.head-wrap map area').mouseenter(function () {
    var atImg = $(this).attr('data-img');
    $('.head-wrap .head-img').attr('src', atImg);
    var dataName = $(this).attr('data-href');
    $(dataName).siblings().removeClass('active');
    $(dataName).addClass('active');
  }).click(function () {
    var dataName = $(this).attr('data-href');
    window.location.href = $(dataName).find("a").eq(0).attr("href"); return false;
  });
  $('.head-wrap map area').mouseleave(function () {
    var atImg = $(this).attr('data-default');
    $('.head-wrap .head-img').attr('src', atImg);
    var dataName = $(this).attr('data-href');
    $(dataName).removeClass('active');
  });
  $(document).on('click', '.js-tab-head', function (e) {
      e.preventDefault();
      $('.js-tabs-item').removeClass('active');
      $(this).parents('.js-tabs-item').addClass('active');
      $('.body-wrap').removeClass('active');
    $('.head-wrap').addClass('active');
  });

  $(document).on('click', '.js-tab-body', function (e) {
    e.preventDefault();
    $('.js-tabs-item').removeClass('active');
    $(this).parents('.js-tabs-item').addClass('active');
    $('.head-wrap').removeClass('active');
    $('.body-wrap').addClass('active');
  });

});

$(function () {
  if ($('.js-index').length) {
    $(document).on('click', 'a.js-page-scroll', function (event) {
      var $anchor = $(this);
      $('.header').removeClass('openmenu');
      $('html, body').stop().animate({
        scrollTop: $($anchor.attr('href')).offset().top
      }, 1000, 'easeInOutExpo');
      event.preventDefault();
    });
  }

});