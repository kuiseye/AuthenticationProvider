import hmac
from os import getenv
from base64 import  urlsafe_b64encode
import hashlib
from uuid import UUID
from random import Random
from datetime import datetime, timedelta
from json import dumps, encoder
import argparse
import os 

def generateJwt(index: int) -> str:
        header_json= dumps({ 'alg':'HS256','typ':'JWT'}).encode('utf-8')
        encodedHead = urlsafe_b64encode(header_json).rstrip(b"=").decode("utf-8")

        payload_json= dumps({'id':UUID(int=rd.getrandbits(128), version=4),
                        "name": "user_" + str(index),
                        "role": "User",
                        "exp": int((datetime.now() + timedelta(days=999)).timestamp()),
                        "iss": "localhost:8080",
                        "aud": "localhost:8080"}, default=str).encode('utf-8')
        encoded_payload = urlsafe_b64encode(payload_json).rstrip(b"=").decode("utf-8")

        hmacValue = hmac.new(bytes(JWT_SECRET), f'{encodedHead}.{encoded_payload}'.encode('utf-8'), hashlib.sha256)

        encoded_signature = urlsafe_b64encode(hmacValue.digest()).rstrip(b"=").decode('utf-8')

        return f'{encodedHead}.{encoded_payload}.{encoded_signature}'


parser = argparse.ArgumentParser("genjwt")

parser.add_argument('-o', '--output', help='output file in form of json array (output.json by default)', default='output.json')
parser.add_argument('-n', '--number', help='number of tokens to generate', type=int, required=True)

args = parser.parse_args()

JWT_SECRET = getenv("JWT_SECRET").encode('utf-8')

rd = Random()
rd.seed(9999)

tokens = []

for i in range(args.number):
        tokens.append(generateJwt(i))

result = dumps(tokens)
fd = os.open(args.output, os.O_WRONLY | os.O_CREAT, 0o600)
with os.fdopen(fd, 'w', encoding='utf-8') as f:
        f.write(result)
os.chmod(args.output, 0o600)
