$(window).load(function() {
    if (/Android|webOS|iPhone|iPad|iPod|BlackBerry/i.test(navigator.userAgent)) {
        $('body').addClass('ios');
    } else {

    };
    $('body').removeClass('loaded');
});
/* viewport width */
function viewport() {
    var e = window,
        a = 'inner';
    if (!('innerWidth' in window)) {
        a = 'client';
        e = document.documentElement || document.body;
    }
    return {
        width: e[a + 'Width'],
        height: e[a + 'Height']
    }
};
/* viewport width */
$(function() {
    /* placeholder*/
    $('input, textarea').each(function() {
        var placeholder = $(this).attr('placeholder');
        $(this).focus(function() {
            $(this).attr('placeholder', '');
        });
        $(this).focusout(function() {
            $(this).attr('placeholder', placeholder);
        });
    });
    /* placeholder*/

    $('.button-nav').click(function() {
        $(this).toggleClass('active'),
            $('.menu').toggleClass('open');
        return false;
    });

    $('.js-link-open-block').click(function() {
        $(this).toggleClass('active'),
            $(this).parents('.js-parent-open-block').find('.js-open-block').slideToggle();
        $('.js-scroll').mCustomScrollbar("update");
        return false;
    });

    if ($('.js-scroll').length) {
        $(".js-scroll").mCustomScrollbar({
            axis: "x",
            advanced: {
                autoExpandHorizontalScroll: true
            }
        });
    };

    if ($('.js-main-slider').length) {
        $('.js-main-slider').slick({
            dots: false,
            infinite: true,
            speed: 600,
            autoplay: false,
            slidesToShow: 1,
            adaptiveHeight: false,
            arrows: false,
            /*prevArrow: '<button type="button" data-role="none" class="slick-prev" aria-label="" tabindex="0" role="button"><span class="arrow-left"></span></button>',
            nextArrow: '<button type="button" data-role="none" class="slick-next" aria-label="" tabindex="0" role="button"><span class="arrow-right"></span></button>',*/
            responsive: [{
                breakpoint: 1280,
                settings: {
                    dots: true,
                }
            }]
        });
    };
    if ($('.js-slider-reviews').length) {
        $('.js-slider-reviews').slick({
            dots: true,
            infinite: true,
            speed: 300,
            slidesToShow: 2,
            slidesToScroll: 2,
            adaptiveHeight: true,
            arrows: false,
            responsive: [{
                breakpoint: 991,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }]
        });
    };
/*    if ($('.js-epil-slider').length) {
        $('.js-epil-slider').slick({
            dots: false,
            infinite: true,
            speed: 400,
            autoplay: true,
            slidesToShow: 1,
            adaptiveHeight: true,
            arrows: false
        });
    };*/

    $('.js-tabs-link').click(function() {
        $(this).parents('.js-tabs').find('.js-tab-cont').addClass('hide-tab');
        $(this).parents('.js-tabs').find('.js-tabs-item').removeClass('active');
        var id = $(this).attr('href');
        $(id).removeClass('hide-tab');
        $(this).parent().addClass('active');
        setTimeout(function() {
            $(window).resize();
        }, 200);
        return false;
    });

    if ($('.styled').length) {
        $('.styled').styler();
    };

    if ($('map').length) {
        $('map').imageMapResize();
    };

    $('.js-turn-body').click(function() {
        $('.body-front').fadeToggle(0);
        $('.body-back').fadeToggle(0);
        setTimeout(function() {
            $(window).resize();
        }, 200);
        return false;
    });
    $('.js-turn-bikini').click(function() {
        $('.bikini-front').fadeToggle(0);
        $('.bikini-back').fadeToggle(0);
        setTimeout(function() {
            $(window).resize();
        }, 200);
        return false;
    });

    $('.box-dummy__main-img map area').mouseenter(function () {
        var atImg = $(this).attr('data-img');
        $('.box-dummy__main-img img').attr('src', atImg);
        var dataName = $(this).attr('data-href');
        $(dataName).siblings().removeClass('active');
        $(dataName).addClass('active');
    }).click(function () {
        var dataName = $(this).attr('data-href');
        window.location.href = $(dataName).find("a").eq(0).attr("href"); return false;
    });
    $('.box-dummy__main-img map area').mouseleave(function () {
        var atImg = $(this).attr('data-default');
        $('.box-dummy__main-img img').attr('src', atImg);
        var dataName = $(this).attr('data-href');
        $(dataName).removeClass('active');
    });

    $('.js-link-area1').mouseenter(function () {
        var atImg = $(this).attr('data-img');
        $('.box-dummy__main-img img').attr('src', atImg);
    });
    $('.js-link-area1').mouseleave(function () {
        var atImg = $(this).attr('data-default');
        $('.box-dummy__main-img img').attr('src', atImg);
    });

    $('.box-dummy__main-img2 map area').mouseenter(function () {
        var atImg2 = $(this).attr('data-img');
        $('.box-dummy__main-img2 img').attr('src', atImg2);
        var dataName2 = $(this).attr('data-href');
        $(dataName2).siblings().removeClass('active');
        $(dataName2).addClass('active');
    }).click(function () {
        var dataName = $(this).attr('data-href');
        window.location.href = $(dataName).find("a").eq(0).attr("href");
    });
    $('.box-dummy__main-img2 map area').mouseleave(function () {
        var atImg2 = $(this).attr('data-default');
        $('.box-dummy__main-img2 img').attr('src', atImg2);
        var dataName2 = $(this).attr('data-href');
        $(dataName2).removeClass('active');
    });

    $('.js-link-area2').mouseenter(function () {
        var atImg2 = $(this).attr('data-img');
        $('.box-dummy__main-img2 img').attr('src', atImg2);
    });
    $('.js-link-area2').mouseleave(function () {
        var atImg2 = $(this).attr('data-default');
        $('.box-dummy__main-img2 img').attr('src', atImg2);
    });

    $('.box-dummy__main-img3 map area').mouseenter(function () {
        var atImg3 = $(this).attr('data-img');
        $('.box-dummy__main-img3 img').attr('src', atImg3);
        var dataName3 = $(this).attr('data-href');
        $(dataName3).siblings().removeClass('active');
        $(dataName3).addClass('active');
    }).click(function () {
        var dataName = $(this).attr('data-href');
        window.location.href = $(dataName).find("a").eq(0).attr("href"); return false;
    });
    $('.box-dummy__main-img3 map area').mouseleave(function () {
        var atImg3 = $(this).attr('data-default');
        $('.box-dummy__main-img3 img').attr('src', atImg3);
        var dataName3 = $(this).attr('data-href');
        $(dataName3).removeClass('active');
    });

    $('.js-link-area3').mouseenter(function () {
        var atImg3 = $(this).attr('data-img');
        $('.box-dummy__main-img3 img').attr('src', atImg3);
    });
    $('.js-link-area3').mouseleave(function () {
        var atImg3 = $(this).attr('data-default');
        $('.box-dummy__main-img3 img').attr('src', atImg3);
    });

    $('.box-dummy__main-img4 map area').mouseenter(function () {
        var atImg4 = $(this).attr('data-img');
        $('.box-dummy__main-img4 img').attr('src', atImg4);
        var dataName4 = $(this).attr('data-href');
        $(dataName4).siblings().removeClass('active');
        $(dataName4).addClass('active');
    }).click(function () {
        var dataName = $(this).attr('data-href');
        window.location.href = $(dataName).find("a").eq(0).attr("href"); return false;
    });
    $('.box-dummy__main-img4 map area').mouseleave(function () {
        var atImg4 = $(this).attr('data-default');
        $('.box-dummy__main-img4 img').attr('src', atImg4);
        var dataName4 = $(this).attr('data-href');
        $(dataName3).removeClass('active');
    });

    $('.js-link-area4').mouseenter(function () {
        var atImg4 = $(this).attr('data-img');
        $('.box-dummy__main-img4 img').attr('src', atImg4);
    });
    $('.js-link-area4').mouseleave(function () {
        var atImg4 = $(this).attr('data-default');
        $('.box-dummy__main-img4 img').attr('src', atImg4);
    });

    $('.box-dummy__main-img5 map area').mouseenter(function () {
        var atImg5 = $(this).attr('data-img');
        $('.box-dummy__main-img5 img').attr('src', atImg5);
        var dataName5 = $(this).attr('data-href');
        $(dataName5).siblings().removeClass('active');
        $(dataName5).addClass('active');
    }).click(function () {
        var dataName = $(this).attr('data-href');
        window.location.href = $(dataName).find("a").eq(0).attr("href"); return false;
    });
    $('.box-dummy__main-img5 map area').mouseleave(function () {
        var atImg5 = $(this).attr('data-default');
        $('.box-dummy__main-img5 img').attr('src', atImg5);
        var dataName5 = $(this).attr('data-href');
        $(dataName5).removeClass('active');
    });

    $('.js-link-area5').mouseenter(function () {
        var atImg5 = $(this).attr('data-img');
        $('.box-dummy__main-img5 img').attr('src', atImg5);
    });
    $('.js-link-area5').mouseleave(function () {
        var atImg5 = $(this).attr('data-default');
        $('.box-dummy__main-img5 img').attr('src', atImg5);
    });

    $(".form-review").submit(function () {
        var type = $("#type-field").val();
        var content = $("#review-field").val();
        var name = $("#name-field").val();
        if (type == "") {
            alert("Выберите категорию отзыва");
            return false;
        }
        if (content == null || content.trim() == "") {
            alert("Введите текст отзыва");
            return false;
        }
        if (name == null || name.trim() == "") {
            alert("Введите ваше имя");
            return false;
        }
        return true;
    });
});

var handler = function() {

    var height_footer = $('footer').height();
    var height_header = $('header').height();
    // $('.content').css({
    //     'padding-bottom': height_footer + 6,
    //     'padding-top': height_header
    // });


    var viewport_wid = viewport().width;

    if (viewport_wid <= 991) {

    }

    $('.js-parent-size-1').each(function() {
        var compLen = (".js-parent-size-2 .table").length;
        if (compLen > 0) {
            $(".js-size-column2 .tr .td").css("height", "auto");
            for (var i = 1; i < 99; i++) {
                var height3 = 0;
                $('.js-size-column2 .tr:nth-child(' + i + ')').each(function() {
                    height3 = height3 > $(this).height() ? height3 : $(this).height();
                });
                $('.js-size-column2 .tr:nth-child(' + i + ') .td').each(function() {
                    $(this).css("height", height3 + "px")
                });
            }
            setTimeout(function() {
                $(".js-size-column2 .tr .td").css("height", "auto");
                for (var i = 1; i < 99; i++) {
                    var height3 = 0;
                    $('.js-size-column2 .tr:nth-child(' + i + ')').each(function() {
                        height3 = height3 > $(this).height() ? height3 : $(this).height();
                    });
                    $('.js-size-column2 .tr:nth-child(' + i + ') .td').each(function() {
                        $(this).css("height", height3 + "px")
                    });
                }
            }, 500);
        }
    });

    $('.js-parent-size-2').each(function() {
        var compLen = (".js-parent-size-2 .table").length;
        if (compLen > 0) {
            $(".js-size-column .tr .td").css("height", "auto");
            for (var i = 1; i < 99; i++) {
                var height2 = 0;
                $('.js-size-column .tr:nth-child(' + i + ')').each(function() {
                    height2 = height2 > $(this).height() ? height2 : $(this).height();
                });
                $('.js-size-column .tr:nth-child(' + i + ') .td').each(function() {
                    $(this).css("height", height2 + "px")
                });
            }
            setTimeout(function() {
                $(".js-size-column .tr .td").css("height", "auto");
                for (var i = 1; i < 99; i++) {
                    var height2 = 0;
                    $('.js-size-column .tr:nth-child(' + i + ')').each(function() {
                        height2 = height2 > $(this).height() ? height2 : $(this).height();
                    });
                    $('.js-size-column .tr:nth-child(' + i + ') .td').each(function() {
                        $(this).css("height", height2 + "px")
                    });
                }
            }, 500);
        }
    });

    $('.js-parent-size-3').each(function() {
        var compLen = (".js-parent-size-3 .table").length;
        if (compLen > 0) {
            $(".js-size-column3 .tr .td").css("height", "auto");
            for (var i = 1; i < 99; i++) {
                var height3 = 0;
                $('.js-size-column3 .tr:nth-child(' + i + ')').each(function() {
                    height3 = height3 > $(this).height() ? height3 : $(this).height();
                });
                $('.js-size-column3 .tr:nth-child(' + i + ') .td').each(function() {
                    $(this).css("height", height3 + "px")
                });
            }
            setTimeout(function() {
                $(".js-size-column3 .tr .td").css("height", "auto");
                for (var i = 1; i < 99; i++) {
                    var height3 = 0;
                    $('.js-size-column3 .tr:nth-child(' + i + ')').each(function() {
                        height3 = height3 > $(this).height() ? height3 : $(this).height();
                    });
                    $('.js-size-column3 .tr:nth-child(' + i + ') .td').each(function() {
                        $(this).css("height", height3 + "px")
                    });
                }
            }, 500);
        }
    });


    $('.js-parent-size-4').each(function() {
        var compLen = (".js-parent-size-4 .table").length;
        if (compLen > 0) {
            $(".js-size-column4 .tr .td").css("height", "auto");
            for (var i = 1; i < 99; i++) {
                var height4 = 0;
                $('.js-size-column4 .tr:nth-child(' + i + ')').each(function() {
                    height4 = height4 > $(this).height() ? height4 : $(this).height();
                });
                $('.js-size-column4 .tr:nth-child(' + i + ') .td').each(function() {
                    $(this).css("height", height4 + "px")
                });
            }
            setTimeout(function() {
                $(".js-size-column4 .tr .td").css("height", "auto");
                for (var i = 1; i < 99; i++) {
                    var height4 = 0;
                    $('.js-size-column4 .tr:nth-child(' + i + ')').each(function() {
                        height4 = height4 > $(this).height() ? height4 : $(this).height();
                    });
                    $('.js-size-column4 .tr:nth-child(' + i + ') .td').each(function() {
                        $(this).css("height", height4 + "px")
                    });
                }
            }, 500);
        }
    });

    if ($('.nav-cont').length) {
        var offset_top = $('.nav-cont').offset().top;
        $(window).scroll(function() {
            if ($(window).scrollTop() > offset_top) {
                $('.nav-cont').addClass("fixed");
            } else {
                $('.nav-cont').removeClass("fixed")
            }
        });

        $(window).load(function() {
            if ($(window).scrollTop() > offset_top) {
                $('.nav-cont').addClass("fixed");
            } else {
                $('.nav-cont').removeClass("fixed")
            }
        });
    };

    var contactLeft = $('.container-fluid').offset().left;
    $('.contact-cont').css({
        'margin-left': contactLeft + 30
    });

};
$(window).bind('load', handler);
$(window).bind('resize', handler);
$(".faq-wrapper .list-faq__item").click(function() {
    var targetIdx = $(this).index();
    $(this).parent().find("li").removeClass("active");
    var parent = $(this).parents(".faq-wrapper").first();
    var container = parent.find(".faq-container");
    var answers = container.find(".faq-answer").hide();
    answers.eq(targetIdx).show();
    $(this).addClass("active");
    return false;
});