ifdef ComSpec
	SEP=\\
else
	SEP=/
endif

DOTNETCMD=dotnet
DOTNETBUILD=$(DOTNETCMD) build
DOTNETCLEAN=$(DOTNETCMD) clean
DOTNETTEST=$(DOTNETCMD) test
DOTNETRUN=$(DOTNETCMD) run

CUI_PROJ_NAME=TryCSharp.Tools.Cui
CUI_PROJ_PATH=$(CUI_PROJ_NAME)$(SEP)$(CUI_PROJ_NAME).csproj

.PHONY: all
all: clean build test

.PHONY: build
build: restore
	$(DOTNETBUILD) --nologo -v q

.PHONY: test
test: restore
	$(DOTNETTEST)

.PHONY: clean
clean: restore
	$(DOTNETCLEAN) --nologo -v q

.PHONY: run
run: clean
	$(DOTNETRUN) --project $(CUI_PROJ_PATH) --onetime

.PHONY: restore
restore:
	$(DOTNETCMD) restore -v q
