import argparse

parser = argparse.ArgumentParser(".env initializer")

parser.add_argument('template', nargs='?', help='input file in form of template required to run the application (.env.template by default)', default='.env.template')
parser.add_argument('-o', '--output', help='output file in form of a .env file (.env by default)', default='.env')

parser.add_argument('-f', '--force', help="forces overwrite to OUTPUT file", action="store_true")  

parser.add_argument('-u', '--db-user', help="db user used for postgres database", required=True)  
parser.add_argument('-p', '--db-password', help="db password used for postgres database", required=True)  

signingKeyGroup = parser.add_mutually_exclusive_group(required=True)

signingKeyGroup.add_argument('-s', '--signing-key', help="raw signing key")  
signingKeyGroup .add_argument('--signing-key-file', help="file containing signing key")  

verificationKeyGroup = parser.add_mutually_exclusive_group(required=True)

verificationKeyGroup.add_argument('-v', '--verification-key', help="raw signing key")  
verificationKeyGroup.add_argument('--verification-key-file', help="file containing verification key")  

args = parser.parse_args()