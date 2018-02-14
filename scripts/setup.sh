#!/bin/sh

echo "Please choose your Silver Iodide backend"

echo "1) PHP, MySQL"
echo "2) PHP, Google App Engine"
echo "3) Go, Google App Engine"

read case;

case $case in
	1) eval git clone https://github.com/Softsurve/agid.php.git build/backend;;
	2) eval git clone https://github.com/Softsurve/agid.php.git build/backend;;
	3) eval git clone https://github.com/dlockamy/agid.go.git build/backend;;
esac


echo "Please choose your Silver Iodide frontend"

echo "1) Facade.js (Javascript/HTML)"
echo "2) BitBoard (.net, WinForms)"

read case;

case $case in
	1) eval git clone https://github.com/dlockamy/facade.js.git build/frontend;;
	2) eval git clone https://github.com/dlockamy/bitboard.git build/frontend;;
esac


