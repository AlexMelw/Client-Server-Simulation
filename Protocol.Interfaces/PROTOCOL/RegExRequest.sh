
#TRANSLATE TEXT
(?:(?<cmd>TRANSLATE)\s+--sourcetext='(?<sourcetext>.+)'\s+--sourcelang='(?<sourcelang>ro|ru|en|unknown)'\s+--targetlang='(?<targetlang>ro|ru|en)')
# ----------------------
TRANSLATE  --sourcetext='some\n text' --sourcelang='ro' --targetlang='en'
TRANSLATE  --sourcetext='some text' --sourcelang='ru' --targetlang='ro'
TRANSLATE  --sourcetext='some text' --sourcelang='en' --targetlang='en'
TRANSLATE  --sourcetext='some text' --sourcelang='unknown' --targetlang='ro'

#REGISTER
(?:(?<cmd>REGISTER)\s+--login='(?<login>[\w]+)'\s+--pass='(?<pass>.+)'\s+--name='(?<name>(?:\w|\s)+)')
# ----------------------
REGISTER  --login='Perkin77' --pass='95myPass95' --name='Denis Starin'
REGISTER  --login='Xeon' --pass='95myPass95' --name='Anton Perkin'
REGISTER  --login='Serg1996' --pass='95myPass95' --name='Petru Bujac'

#AUTHENTICATION LOGIN NAME
(?:(?<cmd>AUTH)\s+(\s+securepassword\s+protectiontype='(?<passprotectiontype>\w+)')?\s+--login='(?<login>\w+)'\s+--pass='(?<pass>.+)')
# ----------------------
AUTH  securepassword protectiontype='MD5' --login='Serg1996' --pass='95myPass95'
AUTH  --login='AlexMayhem' --pass='%%elim5595%%'
AUTH  --login='Serg1996' --pass='95myPass95'

#GET MESSAGE
(?:(?<cmd>GETMSG)\s+--sessiontoken='(?<sessiontoken>(?i:[{(?:]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?))'\s+(?:--(?<translationmode>donottranslate)|(?:--(?<translationmode>translateto)='(?<targetlang>en|ro|ru)')))
# --------------------
GETMSG --sessiontoken='687046DA-BD6C-46CF-ACC5-B67ADACC5866' --donottranslate
GETMSG --sessiontoken='4b6ef0fd-278d-44a9-bc1a-b36d1117d7cd' --translateto='en'
GETMSG --sessiontoken='687046DA-BD6C-46CF-ACC5-B67ADACC5866' --translateto='ro' 
GETMSG --sessiontoken='687046DA-BD6C-46CF-ACC5-B67ADACC5866' --translateto='ru'

#SEND MESSAGE
(?:(?<cmd>SENDMSG)\s+--to='(?<recipient>\w+)'\s+--msg='(?<message>.+)'\s+--sourcelang='(?<sourcelang>en|ro|ru|unknown)'\s+--sessiontoken='(?<sessiontoken>(?i:[{(?:]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?))')
# ----------------------
SENDMSG --to='DenisStarin' --msg='How you doin?' --sourcelang='en' --sessiontoken='687046DA-BD6C-46CF-ACC5-B67ADACC5866'
SENDMSG --to='Ghideon94' --msg='Ce mai faci?' --sourcelang='ro'  --sessiontoken='687046DA-BD6C-46CF-ACC5-B67ABACB5866'
SENDMSG --to='StarTrack77' --msg='Ка поживаешь?' --sourcelang='ru'  --sessiontoken='4b6ef0fd-278d-44a9-bc1a-b36d1117d7cd'
SENDMSG --to='StarTrack77' --msg='Ка поживаешь?' --sourcelang='unknown'  --sessiontoken='687046DA-BD6C-46CF-ACC5-B67ABACC5866'

#HELLO
(?<cmd>HELLO)
# -----------
HELLO