FROM gitpod/workspace-full:latest

USER gitpod

RUN wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \ 
    && sudo dpkg -i packages-microsoft-prod.deb \
    && rm packages-microsoft-prod.deb \
    && sudo apt-get update \
    && sudo apt-get install -y apt-transport-https \
    && sudo apt-get update \
    && sudo apt-get install -y dotnet-sdk-6.0

# https://github.com/gitpod-io/gitpod/issues/5090#issuecomment-954978727
#.NET installed via .gitpod.yml task until the following issue is fixed: https://github.com/gitpod-io/gitpod/issues/5090 
ENV DOTNET_ROOT=
#ENV PATH=$PATH:/tmp/dotnet