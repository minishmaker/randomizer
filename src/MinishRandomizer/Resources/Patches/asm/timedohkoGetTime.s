.equ redTime, yellowTime+4
.equ debugFontWhite, redTime+4
.equ debugFontYellow, debugFontWhite+4
.equ debugFontRed, debugFontYellow+4
.equ greenClock, debugFontRed+4
.equ blueClock, greenClock+4
.equ redClock, blueClock+4
.thumb
push	{r4-r7}
ldr	r7,=#0x203FFE0
@check if we found a green clock
ldr	r0,=#0x2002B38
ldrb	r1,[r0]
mov	r2,#0x03
and	r2,r1
cmp	r2,#0
beq	noGreen
ldr	r4,greenClock
ldr	r5,[r7]
ldr	r6,=#0xFFFFFFFF
sub	r6,r4
cmp	r5,r6
bhi	storeMax1
add	r5,r4
str	r5,[r7]
b	noGreen
storeMax1:
ldr	r6,=#0xFFFFFFFF
str	r6,[r7]
noGreen:

@check if we found a blue clock
mov	r2,#0x0C
and	r2,r1
cmp	r2,#0
beq	noBlue
ldr	r4,blueClock
ldr	r5,[r7]
ldr	r6,=#0xFFFFFFFF
sub	r6,r4
cmp	r5,r6
bhi	storeMax2
add	r5,r4
str	r5,[r7]
b	noBlue
storeMax2:
ldr	r6,=#0xFFFFFFFF
str	r6,[r7]
noBlue:

@check if we found a red clock
mov	r2,#0x30
and	r2,r1
cmp	r2,#0
beq	noRed
ldr	r4,redClock
ldr	r5,[r7]
cmp	r4,r5
bhi	set0
sub	r5,r4
str	r5,[r7]
b	noRed
set0:
mov	r4,#0
str	r4,[r7]
noRed:

@reset flags
mov	r2,#0xC0
and	r2,r1
strb	r2,[r0]

@change the font color
ldr	r0,[r7]
ldr	r1,redTime
cmp	r0,r1
bls	red
ldr	r1,yellowTime
cmp	r0,r1
bls	yellow

white:
ldr	r0,debugFontWhite
b	load

yellow:
ldr	r0,debugFontYellow
b	load

red:
ldr	r0,debugFontRed

load:
ldr	r1,=#0x600C600
ldr	r2,=#0x600C760
debug:
ldr	r3,[r0]
str	r3,[r1]
add	r0,#4
add	r1,#4
cmp	r1,r2
bne	debug

ldr	r0,[r7]
cmp	r0,#0
beq	end
sub	r0,#1
str	r0,[r7]
end:
pop	{r4-r7}
bx	lr
.align
.ltorg
yellowTime:
@WORD yellowTime
@WORD redTime
@POIN debugFontWhite
@POIN debugFontYellow
@POIN debugFontRed
@WORD greenClock
@WORD blueClock
@WORD redClock
