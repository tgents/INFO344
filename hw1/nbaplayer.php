<?php
	class NbaPlayer {
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
        	$this->name = $n;
            $this->team = $t;
            $this->games = $g;
            $this->fgm = $sm;
            $this->fga = $sa;
            $this->threem = $tm;
            $this->threea = $ta;
            $this->ftm = $fm;
            $this->fta = $fa;
            $this->rebounds = $reb;
            $this->assists = $as;
            $this->steals = $st;
            $this->blocks = $bl;
            $this->ppg = $pts; 
        }

        public function printPlayer() {
            echo "<h2>$this->name</h2>";
            echo "<h3>$this->team</h3>";

            echo "<p>";

            echo "Games Played: $this->games";
            echo "<br>Shots per game (made/attempts): $this->fgm/$this->fga";
            echo "<br>Threes per game (made/attempts): $this->threem/$this->threea";
            echo "<br>Average points per game: $this->ppg";

            echo "</p>";
        }
    }
?>