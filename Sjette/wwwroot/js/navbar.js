$(function () {
    $(window).on('scroll', function () {
        if ($(window).scrollTop() > ($(window).height() - 100)) {
            $('.navbar').addClass('active');
        } else {
            $('.navbar').removeClass('active');
        }
    });
});
