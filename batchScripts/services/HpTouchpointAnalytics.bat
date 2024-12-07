
:: Stop HP Insights Analytics and change startup type to disabled

@ECHO OFF

SC STOP HpTouchpointAnalyticsService 2:64:256 "Free up ram"

SC CONFIG HpTouchpointAnalyticsService START= DISABLED

PAUSE

EXIT
