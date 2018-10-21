FROM microsoft/aspnetcore:2.0
ENV DEBIAN_FRONTEND=noninteractive
WORKDIR /app
STOPSIGNAL SIGTERM
EXPOSE 80

COPY . /app
RUN apt-get update && apt-get install -y ca-certificates
COPY *.crt  /usr/local/share/ca-certificates/
RUN update-ca-certificates


RUN \
  DUMB_INIT_VERSION=1.2.1 && \
  DUMB_INIT_SHA256SUM=057ecd4ac1d3c3be31f82fc0848bf77b1326a975b4f8423fe31607205a0fe945 && \
  curl -fsSLo /sbin/dumb-init "https://github.com/Yelp/dumb-init/releases/download/v${DUMB_INIT_VERSION}/dumb-init_${DUMB_INIT_VERSION}_amd64" && \
  echo "${DUMB_INIT_SHA256SUM} */sbin/dumb-init" | sha256sum -c - && \
  chmod +x /sbin/dumb-init
RUN mkdir -p /app/etc/distribution/keys
ENTRYPOINT [ "/sbin/dumb-init", "--" ]

CMD ["dotnet", "Xebia.Service.Host.dll"]
