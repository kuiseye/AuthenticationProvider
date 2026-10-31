from inv_env_parser import args
from dotenv import dotenv_values
import os
import sys

def loadKeyFromFile(path : str) -> str:
    fd = os.open(path, os.O_RDONLY)
    with os.fdopen(fd, "r", encoding="utf-8") as f:
        return f.read()

DB_USER_ENV_NAME="POSTGRES_USER"
DB_PASSWOR_ENV_NAME="POSTGRES_PASSWORD"
SIGNING_KEY_ENV_NAME="TokenKeyPairSettings__SigningKey"
VERIFICATION_KEY_ENV_NAME="TokenKeyPairSettings__VerificationKey"

dbUser = args.db_user
dbPassword = args.db_password

signingKey = args.signing_key if args.signing_key != None else loadKeyFromFile(args.signing_key_file)
verificationKey = args.verification_key if args.verification_key != None else loadKeyFromFile(args.verification_key_file)


values = dotenv_values('.env.template')

values[DB_USER_ENV_NAME]=dbUser
values[DB_PASSWOR_ENV_NAME]=dbPassword
values[SIGNING_KEY_ENV_NAME]=signingKey
values[VERIFICATION_KEY_ENV_NAME]=verificationKey

if os.path.exists(args.output) and not args.force:
    print(f"{args.output} already exists (use --force to overwrite)", file=sys.stderr)
    exit(1)

fd = os.open(args.output, os.O_WRONLY | os.O_CREAT | os.O_TRUNC, 0o600)
with os.fdopen(fd, "w", encoding="utf-8") as f:
    for key, value in values.items():
        f.write(f"{key}=\"{value}\"\n")
os.chmod(args.output, 0o600)

