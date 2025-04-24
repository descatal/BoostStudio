# syntax=docker/dockerfile:1

ARG NODE_VERSION=20
ARG PNPM_VERSION=10.4.1

FROM node:${NODE_VERSION}-slim as base
RUN --mount=type=cache,target=/root/.npm \
    npm install -g pnpm@${PNPM_VERSION}
WORKDIR /usr/src/app
EXPOSE 1420

RUN apt update
RUN apt install wget -y
RUN wget https://github.com/browsh-org/browsh/releases/download/v1.8.0/browsh_1.8.0_linux_amd64.deb
RUN apt install ./browsh_1.8.0_linux_amd64.deb -y

# Download dependencies as a separate step to take advantage of Docker's caching.
# Leverage a cache mount to /root/.local/share/pnpm/store to speed up subsequent builds.
# Leverage a bind mounts to package.json and pnpm-lock.yaml to avoid having to copy them into
# into this layer.
FROM base as dev
RUN --mount=type=bind,source=package.json,target=package.json \
    --mount=type=bind,source=pnpm-lock.yaml,target=pnpm-lock.yaml \
    --mount=type=cache,target=/root/.local/share/pnpm/store \
    pnpm install
COPY . .
CMD pnpm dev --host