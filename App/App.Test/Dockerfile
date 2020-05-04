FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

COPY . .

RUN dotnet build

ENTRYPOINT ["dotnet", \
    "test", \
    "--no-restore", \
    "--no-build", \
    "--verbosity:normal", \
    "--logger", \
        "trx;LogFileName=test.trx", \
    "--results-directory", \
        "test-results"]