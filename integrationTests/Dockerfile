FROM node:17

WORKDIR /tests
COPY package.json package.json
COPY yarn.lock yarn.lock

RUN yarn install

COPY . .
RUN yarn build

ENTRYPOINT ["tail", "-f", "/dev/null"]