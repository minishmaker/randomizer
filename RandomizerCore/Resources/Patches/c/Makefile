# === begin config block ===
# change these variables to configure build
WARNINGS := -Wall -Wextra -Werror
CFLAGS := -O2 -g -fno-inline
# ==== end config block ====

CC := arm-none-eabi-gcc

CFLAGS := $(CFLAGS) $(WARNINGS) -Wno-multichar -Wno-builtin-declaration-mismatch -mcpu=arm7tdmi -mthumb -mlong-calls -I $(REPO)/tools/agbcc/include -I $(REPO)/include -nostdinc -undef -DEU -DREVISION=0 -DLANGUAGE=ENGLISH

C_SRCS := $(wildcard *.c)
C_EVENTS := $(patsubst %.c,%.event,$(C_SRCS))

.PHONY: all clean tidy format
all: $(C_EVENTS)

# events not deleted because we keep them around
clean:
	rm -f *.s
	rm -f *.o

tidy: clean
	rm -f *.event

format:
	clang-format -i *.c

%.event: %.o eu_symbols.json
	eac compile -o $@ --symbols eu_symbols.json $<

%.o: %.c
	$(CC) $(CFLAGS) -o $@ -c $<

# for testing
%.s: %.c
	$(CC) $(CFLAGS) -o $@ -S $<

eu_symbols.json: $(REPO)/tmc_eu.elf
	eac extract_symbols -o $@ $<
