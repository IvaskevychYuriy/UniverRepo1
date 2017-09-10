var carouselItems = $(".carouselItem");
var itemsCount = carouselItems.length;
var currItem = 0;

$("#prev").click( function() {
   carouselItems.eq(currItem).removeAttr("id");
   
   if (currItem === 0) {
       currItem = itemsCount - 1;
   }
   else {
       --currItem;
   }

   carouselItems.eq(currItem).attr("id", "currentItem");
});

$("#next").click( function() {
   carouselItems.eq(currItem).removeAttr("id");
   
   if (currItem === itemsCount - 1) {
       currItem = 0;
   }
   else {
       ++currItem;
   }

   carouselItems.eq(currItem).attr("id", "currentItem");
});

$("#colorPicker").on("change", function() {
   $("#fader").css("background-color", $("#colorPicker").val());
});

$("#opacityPicker").on("change", function() {
   $("#fader").css("opacity", $("#opacityPicker").val() / 100);
   $("#opac").text($("#opacityPicker").val() / 100);
});

$("#showSettings").click( function () {
   $("#settings").slideToggle("fast");
});

$(".galleryItem img").on({
   mouseover: function() {
       $(this).parent().next(".tooltip").css({display: 'block'});
       },
   mouseout: function() {
       $(this).parent().next(".tooltip").css({display: 'none'});
       },
});
$(".tooltip").on({
   mouseover: function() {
       $(this).css({display: 'block'});
       },
   mouseout: function() {
       $(this).css({display: 'none'});
       },
});