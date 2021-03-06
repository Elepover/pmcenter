FROM mcr.microsoft.com/dotnet/core/runtime:3.1-alpine

ARG LOCALE=en
WORKDIR /opt

RUN mkdir pmcenter \
    && cd pmcenter \
    && wget https://see.wtf/pmcenter-update -O pmcenter.zip \
    && unzip pmcenter.zip \
    && rm pmcenter.zip
ADD locales/pmcenter_locale_$LOCALE.json /opt/pmcenter/pmcenter_locale.json

CMD ["dotnet","/opt/pmcenter/pmcenter.dll"]