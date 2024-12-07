
:: Stop XTUOCDriverService and change startup type to manual

@ECHO OFF

SC STOP XTU3SERVICE 2:64:256 "Free up ram"

SC CONFIG XTU3SERVICE START= DEMAND

PAUSE

EXIT
