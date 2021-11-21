FROM gitpod/workspace-full:latest

USER gitpod

# https://github.com/gitpod-io/gitpod/issues/5090#issuecomment-954978727
#.NET installed via .gitpod.yml task until the following issue is fixed: https://github.com/gitpod-io/gitpod/issues/5090
ENV DOTNET_ROOT=/tmp/dotnet
ENV PATH=$PATH:/tmp/dotnet