var playerNodes = [];
var request = new XMLHttpRequest();
request.open('GET', './givemethestuff.php', true);

request.onload = function() {
  if (request.status >= 200 && request.status < 400) {
    // Success!
    var text = request.responseText;
    console.log(text);
    var data = JSON.parse(text);
    var plays = [];
    for(var i = 0; i < data.length; i++) {
      // console.log(data[i]['Name']);
      var calc = "Games Played: " + data[i]['GP'] + "<br>";
      calc +=  "Shots/game (Made/Attempts): " + data[i]['FG_M'] + "/" + data[i]['FG_A'] + "<br>";
      calc +=  "Threes/game (Made/Attempts): " + data[i]['3PT_M'] + "/" + data[i]['3PT_A'] + "<br>";
      calc +=  "Points/game: " + data[i]['PPG'] + "<br>";
      plays.push({name: data[i]['Name'], team: data[i]['Team'], stats: calc});
    }

    playerNodes = plays;
    addPlayers(plays);

  } else {
    // We reached our target server, but it returned an error

  }
};

request.onerror = function() {
  // There was a connection error of some sort
};

request.send();

function populateNode(node, player) {
  // console.log(player);
  // console.log(node.childNodes[0]);
  node.childNodes[0].innerText = player['name'];
  node.childNodes[1].innerText = player['team'];
  node.childNodes[2].innerHTML = player['stats'];
  return node;
}

function addPlayers(players) {
  var display = document.getElementById("players");
  display.innerHTML = "";
  for(var i = 0; i < players.length; i++) {
    var playerNode = createNode();
    playerNode = populateNode(playerNode, players[i]);
    display.appendChild(playerNode);
  }
}

function createNode(){
  var node = document.createElement("div");
  node.setAttribute("id", "player");
  node.setAttribute("class", "col-md-3");
  var name = document.createElement("h2");
  var team = document.createElement("h3");
  var stats = document.createElement("p");
  name.setAttribute("id", "name");
  team.setAttribute("id", "team");
  stats.setAttribute("id", "stats");
  node.appendChild(name);
  node.appendChild(team);
  node.appendChild(stats);
  return node;
}

function search(){
  var input = document.getElementById("player");
  var playStr = input.value.toLowerCase().trim();
  var showThese = [];
  var count = 0;
  for(var i = 0; i < playerNodes.length; i++) {
    var player = playerNodes[i].name.toLowerCase();
    var wordFound = player.indexOf(playStr);
    if(wordFound > -1) {
      showThese.push(playerNodes[i]);
      count++;
    }
  }
  if(count == 0){
    var display = document.getElementById("players");
    display.innerHTML = "<h2>Sorry. We could not find that player. :(</h2>";
  } else {
    addPlayers(showThese);
  }
}