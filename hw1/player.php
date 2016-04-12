<?php
	class Player {
        private $name = "N/A";
        private $team = "N/A";
        private $games = "N/A";
        private $fgm = "N/A";
        private $fga = "N/A";
        private $threem = "N/A";
        private $threea = "N/A";
        private $ftm = "N/A";
        private $fta = "N/A";
        private $rebounds = "N/A";
        private $assists = "N/A";
        private $steals = "N/A";
        private $blocks = "N/A";
        private $ppg = "N/A";

        function __construct($n, $t, $g, $sm, $sa, $tm, $ta, $fm, $fa, $reb, $as, $st, $bl, $pts) {
        	$name = $n;
            $team = $t;
            $games = $g;
            $fgm = $sm;
            $fga = $sa;
            $threem = $tm;
            $threea = $ta;
            $ftm = $fm;
            $fta = $fa;
            $rebounds = $reb;
            $assists = $as;
            $steals = $st;
            $blocks = $bl;
            $ppg = $pts; 
        }

        public function printPlayer() {
            echo "<h2>$name</h2>";
            echo "<h3>$team</h3>";

            echo "<p>";

            echo "Games Played: $games";
            echo "<br>Shots per game (made/attempts): $fgm/$fga";
            echo "<br>Threes per game (made/attempts): $threem/$threea";
            echo "<br>Average points per game: $ppg";

            echo "</p>";
        }
    }
?>