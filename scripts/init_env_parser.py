import argparse

parser = argparse.ArgumentParser(".env initializer")

parser.add_argument('env_folder', nargs='?', help='input file in form of template required to run the application (.env.template by default)', default='./env/')

parser.add_argument('-f', '--force', help="forces overwrite to OUTPUT file", action="store_true")  

parser.add_argument('-u', '--db-user', help="db user used for postgres database", required=True)  
parser.add_argument('-p', '--db-password', help="db password used for postgres database", required=True)  

signingKeyGroup = parser.add_mutually_exclusive_group(required=True)

signingKeyGroup.add_argument('-s', '--signing-key', help="raw signing key")  
signingKeyGroup .add_argument('--signing-key-file', help="file containing signing key")  

verificationKeyGroup = parser.add_mutually_exclusive_group(required=True)

verificationKeyGroup.add_argument('-v', '--verification-key', help="raw signing key")  
verificationKeyGroup.add_argument('--verification-key-file', help="file containing verification key")  

parser.add_argument('-H', '--host', help="domain name of the application", required=True)  
parser.add_argument('-P','--port', help="port to expose on the host", required=True)  

args = parser.parse_args()