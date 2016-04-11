function search(){
  var searchstr = document.getElementById("player").value;
  document.location = "tilt.php?player=" + searchstr.toLowerCase().trim();
}