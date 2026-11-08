from init_env_parser import args
from dotenv import dotenv_values
import dotenv.main
import os
import sys

RELATIVE_DB_ENV_FILE_PATH=".env.db"
RELATIVE_AUTH_SECRETS_ENV_FILE_PATH=".env.auth.secrets"
TEMLATE_ENDING=".template"
####################################################################33
DB_USER_ENV_NAME="POSTGRES_USER"
DB_PASSWOR_ENV_NAME="POSTGRES_PASSWORD"
SIGNING_KEY_ENV_NAME="TokenKeyPairSettings__SigningKey"
VERIFICATION_KEY_ENV_NAME="TokenKeyPairSettings__VerificationKey"

def loadKeyFromFile(path : str) -> str:
    fd = os.open(path, os.O_RDONLY)
    with os.fdopen(fd, "r", encoding="utf-8") as f:
        return f.read()

def writeEnvsToFile(file_path: str, env_values: dict[str,str|None] )->None:
    fd = os.open(file_path, os.O_WRONLY | os.O_CREAT | os.O_TRUNC, 0o600)
    with os.fdopen(fd, "w", encoding="utf-8") as f:
        for key, value in env_values.items():
            f.write(f"{key}=\"{value}\"\n")
    os.chmod(file_path, 0o600)

folder = args.env_folder

db_file_path = folder + RELATIVE_DB_ENV_FILE_PATH
auth_file_path = folder + RELATIVE_AUTH_SECRETS_ENV_FILE_PATH
if os.path.exists(db_file_path) and not args.force:
    print(f"{db_file_path} already exists (use --force to overwrite)", file=sys.stderr)
    exit(1)

if os.path.exists(auth_file_path) and not args.force:
    print(f"{auth_file_path} already exists (use --force to overwrite)", file=sys.stderr)
    exit(1)



dbUser = args.db_user
dbPassword = args.db_password

db_values = dotenv_values(db_file_path + TEMLATE_ENDING, interpolate=False)

db_values[DB_USER_ENV_NAME]=dbUser
db_values[DB_PASSWOR_ENV_NAME]=dbPassword

################################################################################
signingKey = args.signing_key if args.signing_key != None else loadKeyFromFile(args.signing_key_file)
verificationKey = args.verification_key if args.verification_key != None else loadKeyFromFile(args.verification_key_file)

auth_values = dotenv_values(auth_file_path + TEMLATE_ENDING, interpolate=False)

auth_values[SIGNING_KEY_ENV_NAME]=signingKey
auth_values[VERIFICATION_KEY_ENV_NAME]=verificationKey

################################################################################

writeEnvsToFile(db_file_path,db_values)
writeEnvsToFile(auth_file_path, auth_values)