<?php
	class Book {
        private $name = "[no title]";
        private $price = 0;

        function __construct($n, $p) {
            $this->name = $n;
            $this->price = $p;
        }

        public function getPrice() {
            return $this->price;
        }

        public function getName() {
            return $this->name;
        }

        public static function getDefaultBooks() {
            $books = array();
            $books[] = new Book("In Search of Lost Time", 20);
            $books[] = new Book("Ulysses", 10);
            $books[] = new Book("Don Quixote", 50);
            $books[] = new Book("Adventures of Albert", 30);
            $books[] = new Book("Moby Dick", 40);
            return $books;
        }
    }
?>