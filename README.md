# IwSK
Interfejsy w Systemach Komputerowych - Komunikacja przez port znakowy

#############################################################################################################################

PROWADZĄCY

  Wojciech Mielczarek

CELE LABORATORIUM

  Celem ćwiczenia jest praktyczne poznanie zasad komunikacji urządzeń za pośrednictwem interfejsu RS-232, protokołów 
  komunikacyjnych opartych na łączu znakowym oraz budowy i programowania kontrolera interfejsu szeregowego w komputerze PC.
  
TREŚĆ LABORATORIUM
  
  Ćwiczenie obejmuje napisanie i uruchomienie dwóch programów kontrolujących komunikację na szeregowym łączu znakowym.
  Pierwszy program obsługuje transmisję pomiędzy dwoma urządzeniami DTE (np. komputerami lub komputerem i urządzeniem
  pomiarowym) połączonymi kablem połączenia bezmodemowego. Drugi program nadzoruje pracę systemu (sieci obiektowej) 
  opartego na magistrali RS-485 i protokole MODBUS.

LITERATURA
  
  1. W. Mielczarek: Szeregowe interfejsy cyfrowe, Helion 1993
  2. W. Mielczarek: Urządzenia i systemy kompatybilne ze standardem SCPI, Helion 1999
  3. W. Mielczarek: Tłumienie zakłóceń i ochrona informacji w systemach pomiarowych, 
     skrypt Politechniki Śląskiej nr 1921, Gliwice 1995

NARZĘDZIA
  
  Stanowisko laboratoryjne składające się z następujących urządzeń:
  - trzech komputerów wyposażonych w porty RS-232,
  - urządzenia pomiarowego wyposażonego w port RS-232,
  - dwóch koncentratorów RS-232/4xRS-232,
  - konwertera RS-232/RS-485,
  - dwóch liczników wyposażonych w łącze RS-485,
  - okablowania połączenia bezmodemowego oraz magistrali RS-485.

#############################################################################################################################
KONTROLA KOMUNIKACJ NA PORCIE ZNAKOWYM

  Funkcje (OB – funkcja obligatoryjna, OP - funkcja opcjonalna)
  1. Konfiguracja łącza do komunikacji
    1.1. Wybór portu (połączony ze sprawdzeniem obecności portu) [OB]
    1.2. Ustawienie parametrów transmisyjnych [OB]
          - szybkość (od 150 bit/s do 115 kb/s),
          - format znaku (7 lub 8 bitowe pole danych, kontrola: E, O lub N, 1 lub 2 bity stop)
    1.3. Kontrola przepływu [OB]
          - brak kontroli przepływu,
          - „sprzętowa” (handshake): DTR/DSR, RTS/CTS,
          - “programowa”: XON/XOFF
    1.4. Przepływ sterowany „ręcznie”: – możliwość ustawienia „na życzenie” wyjść DTR lub RTS, 
         monitoring stanu wejść DSR, CTS [OP].
    1.5. Wybór terminatora [OB]
          - brak terminatora,
          - terminator standardowy (CR, LF, CR-LF),
          - terminator „własny” 1 lub 2 znakowy
  2. Nadawanie [OB]
  3. Odbiór [OB]
  4. Transakcja (nadawanie i odbiór przy ustawionym ograniczeniu czasowym oczekiwania na odpowiedź [OP]
  5. PING: kontrola sprawności łącza wraz z pomiarem czasu „round trip delay” [OB]
  6. Tryby transmisji:
    6.1. Tekstowy [OB]
      Nadawanie:
        Wprowadzanie znaków alfanumerycznych do bufora transmisyjnego połączone z ich
        prezentacją w oknie „Nadawanie” i możliwością edycji. Po wydaniu komendy
        „wyślij” wysłanie bufora na łącze z dopisaniem na końcu terminatora.
      Odbiór:
        Prezentacja odebranych znaków alfanumerycznych w oknie „Odbiór”.
    6.2. Binarny [OP]
      Opcja przeznaczona do wysłania dowolnych bajtów w prowadzanych w kodzie
      heksadecymalnym. Wymaga wykonania prostego heks edytora.

      Nadawanie:
      2
      Wprowadzanie bajtów binarnych w kodzie heksadecymalnym do bufora
      transmisyjnego połączone z ich prezentacją w oknie „Nadawanie” i możliwością
      edycji. Po wydaniu komendy „wyślij” wysłanie bufora na łącze z dopisaniem na
      końcu terminatora.
      Ustawienie dostępu do pliku binarnego i jego wysłanie.
      Prezentacja odebranych bajtów binarnych w kodzie heksadecymalnym w oknie
      „Odbiór”.
  7. Autobauding [OP]
    Procedura automatycznej identyfikacji parametrów transmisyjnych ustawionych po
    drugiej stronie łącza, prezentacja zidentyfikowanych wartości.

Testowanie programu
  Należy 2 komputery połączyć kablem bezmodemowym i sprawdzić zaimplementowane
  funkcjonalności. Każda sekcja musi dysponować własnym kablem połączeniowym
  wykonanym zgodnie ze specyfikacją przedstawioną na rys.1.

                                   DTE                                                  DTE
                             ______________                                        ______________
                            |           TxD| ---------------->>>----------------  |RxD           | 
                            |           RxD| ----------------<<<----------------  |TxD           | 
                            |              |                                      |              |
                            |           DTR| ---------------->>>----------------  |DSR           | 
                            |           DSR| ----------------<<<----------------  |DTR           | 
                            |              |                                      |              |
                            |           RTS| ---------------->>>----------------  |CTS           | 
                            |           CTS| ----------------<<<----------------  |RTS           | 
                            |              |                                      |              |
                            |            SG| -----------------------------------  |SG            | 
                            |______________|                                      |______________|
                            
                                        Rys.1. Połączenie bezpośrednie („bezmodemowe”). 
                                        Kabel zakończony złączami DB9F po obu stronach.

#############################################################################################################################
OBSŁUGA KOMUNIKACJI POMIĘDZY STACJAMI SYSTEMU MODBUS PRACUJĄCYMI W TRYBIE ASCII LUB RTU

  Program implementuje funkcje warstw fizycznej i łącza danych sieci obiektowej opartej na
  szeregowym łączu znakowym i protokole MODBUS-ASCII (OB.) lub Modbus RTU (OP)
  
  Funkcje programu:
  1. Wybór rodzaju stacji protokołu MODBUS: Master lub Slave
    1.1. Stacja Master:
        - Realizacja transakcji adresowanej i rozgłoszeniowej
        - Zdefiniowanie ramki przez operatora: adresu stacji slave, rozkazu i argumentów, “Automatyczne” 
          wyznaczenie LRC i dopisanie znacznika końca,
        - Wysłanie ramki na żądanie operatora.
        
        Pracę stacji MASTER kontrolują 3 parametry:
          - ograniczenie czasowe (timeout) na wykonanie transakcji 
            (ustawiane w zakresie 0 do 10 s z rozdzielczością 100 ms),
          - liczba retransmisji w przypadku niepowodzenia transakcji (ustawiana w zakresie 0 do 5)
          - ograniczenie czasowe (0 do 1 s co 10 ms) na odstęp pomiędzy znakami ramki 
            (parametr związany z wymaganiem ciągłości ramki, kontrolowany podczas odbioru odpowiedzi).
    1.2. Stacja Slave
        - Odbiór zapytania,
        - Sprawdzenie poprawności ramki,
        - Sprawdzenie adresu przeznaczenia,
        - Wykonanie rozkazu,
        - Odesłanie odpowiedzi normalnej lub szczególnej.
        
        Pracę stacji Slave kontrolują dwa parametry:
          - adres stacji (ustawiany w zakresie 1 – 247)
          - ograniczenie czasowe (0 do 1 s co 10 ms) na odstęp pomiędzy znakami ramki
            (parametr związany z wymaganiem ciągłości ramki, kontrolowany podczas odbioru zapytania).
  2. Warstwa aplikacji
      Wystarczy zaimplementować tylko dwa, niestandardowe rozkazy:
        - kod rozkazu = 1 – wysłanie tekstu ze stacji Master do stacji Slave
          Tekst wpisany do okna edycyjnego w stacji Master zostaje przekazany (zapisany) do
          stacji Slave i wyświetlony w oknie „Tekst odebrany” w stacji Slave. Rozkaz możliwy do
          wykonania w ramach transakcji adresowanej, jak i rozgłoszeniowej.
        - kod rozkazu = 2 – odczyt tekstu ze stacji Slave i i wyświetlenie tekstu w oknie „Tekst
          odebrany” w stacji Master. Rozkaz możliwy do wykonania tylko w ramach transakcji adresowanej.
  
  UWAGA:
  W stacji MASTER i w stacji SLAVE należy umożliwić podgląd ramek wysłanej oraz odebranej w kodzie heksadecymalnym.


               _____                                     RS-485                                      _____
              | R_T |_______________________________________________________________________________| R_T |
              |_____|     |                  |              |              |                 |      |_____|
                        __|___           ____|____      ____|____      ____|____         ____|____  
                       |RS-485|         |         |    |         |    |         |       |         |
                       |______|         | SLAVE 1 |    | SLAVE 2 |    | SLAVE 3 | . . . | SLAVE N | 
                       |RS-232|         |_________|    |_________|    |_________|       |_________|
                       |______|
                          |
                   _______|_______
                  ||    |COM|    ||
                  ||             ||
                  ||   MASTER    ||
                  ||_____________||                        
                 /+++++++++++++++++\
                /___________________\


                                  Rys.2. System MODBUS oparty na magistrali RS-485


                               MASTER                                               SLAVE
                           ______________                                        ______________
                          |              | ---------------->>>----------------  |              | 
                          |   ADRES      |                                      |   ADRES      |
                          |              |                                      |              |
                          |   ROZKAZ     |                                      |   ROZKAZ     | 
                          |              |                                      |              | 
                          |   DANE       |                                      |   DANE       |
                          |              |                                      |              | 
                          |   SUMA       |                                      |   SUMA       | 
                          |   KONTROLNA  |                                      |   KONTROLNA  |
                          |              | ----------------<<<----------------  |              | 
                          |______________|                                      |______________|
                          
                                                    Rys.3. Transakcja
                                                    
#############################################################################################################################
