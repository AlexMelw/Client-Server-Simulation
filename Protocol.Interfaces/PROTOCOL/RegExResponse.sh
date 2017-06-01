
#HELLO RESULT
(?:(?<statuscode>200)\s+(?<statusdesc>OK)\s+(?<cmd>HELLO))
# ------------------------
200 OK HELLO

# TRANSLATE RESULT
(?:(?<statuscode>200)\s+(?<statusdesc>OK)\s+(?<cmd>TRANSLATE)\s+--res='(?<res>.+)')
# --------------------------
200 OK TRANSLATE --res='Как дела?'
200 OK TRANSLATE --res='Salut. How you doin?'

# REGISTER RESULT
(?:(?<statuscode>\d{3})\s+(?<statusdesc>(?:OK|ERR))\s+(?<cmd>REGISTER)\s+--res='(?<res>.+)')
# --------------------------
200 OK REGISTER --res='User registered successfully'
502 ERR REGISTER --res='User already exists'

#AUTHENTICATION RESULT
(?:(?:(?<statuscode>\d{3})\s+(?<statusdesc>OK)\s+(?<cmd>AUTH)\s+--res='(?<res>.+)'\s+--sessiontoken='(?<sessiontoken>(?i:[{(?:]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?))')|(?:(?<statuscode>\d{3})\s+(?<statusdesc>ERR)\s+(?<cmd>AUTH)\s+--res='(?<res>.+)'))
# --------------------------
201 OK AUTH --res='User authenticated successfully' --sessiontoken='4b6ef0fd-278d-44a9-bc1a-b36d1117d7cd'
201 OK AUTH --res='User authenticated successfully' --sessiontoken='687046DA-BD6C-46CF-ACC5-B67ADACC5866'
401 ERR AUTH --res='login or password incorrect'
#201 Authorized
#401 Unauthorized

# SEND MESSAGE RESULT
(?:(?<statuscode>\d{3})\s+(?<statusdesc>OK|ERR)\s+(?<cmd>SENDMSG)\s+--res='(?<res>.+)')
# --------------------------
200 OK SENDMSG --res='Message sent successfully'
512 ERR SENDMSG --res='Inexistent recipient'
511 ERR SENDMSG --res='Athentication required'

# GET MESSAGE RESULT
(?:(?<statuscode>\d{3})\s+(?<statusdesc>ERR)\s+(?<cmd>GETMSG)\s+--res='(?<res>.+)')|(?:(?<statuscode>\d{3})\s+(?<statusdesc>OK)\s+(?<cmd>GETMSG)\s+--senderid='(?<senderid>\w+)'\s+--sendername='(?<sendername>.+)'\s+--msg='(?<message>.+)')
# --------------------------
200 OK GETMSG --senderid='msgSenderName1259' --sendername='Buldumac Vasile' --msg='{msg.TextBody}'
200 OK GETMSG --senderid='msg_SenderName' --sendername='Buldumac Oleg' --msg='{translatedText'
513 ERR GETMSG --res='Message Box is empty'
511 ERR GETMSG --res='Athentication required'

#SHUTDOWN RESULT
(?:(?<statuscode>200)\s+(?<statusdesc>OK)\s+(?<cmd>SHUTDOWN)\s+--res='(?<res>.+)')
# ------------------------
200 OK SHUTDOWN --res='UDP Server Halted'
200 OK SHUTDOWN --res='TCP Server Halted'


# ------------------------- TEMP ----------------------------

#AUTHENTICATION RESULT
#(?:(?<statuscode>(?:\d{3}))\s+(?<statusdesc>(?:OK|ERR))\s+(?<cmd>AUTH)\s+--res='(?<resvalue>unauthorized|authorized)')
# -------------------------
#201 Authorized --auth-token='687046DA-BD6C-46CF-ACC5-B67ADACC5866'
#401 Unauthorized --ERR='Login or password is wrong'
#400 Bad Request --ERR='Not applicable'
#200 OK TRANSLATE --TEXT='Careva text'

#REGISTER
#200 OK AUTH --res='User authenticated successfully' AuthToken='687046DA-BD6C-46CF-ACC5-B67ADACC5866'
#502 ERR REGISTER --res='User already exists'
#\s+--guid='(?<guid>[{(?:]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?)'
#AUTH --guid='687046DA-BD6C-46CF-ACC5-B67ADACC5866' --name='VeaceslavWD'
#REGISTER --guid='687046DA-BD6C-46CF-ACC5-B67ADACC5866' --name='HelloMotto'
#REGISTER --guid='687046DA-BD6C-46CF-ACC5-B67ADACC5866' --name='Serg1996'
#TRANSLATE  --sourcetext='Salut! Ce mai faci?' --sourcelang='ro' --targetlang='en'