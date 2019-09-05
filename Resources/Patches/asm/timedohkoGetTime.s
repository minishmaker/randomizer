.equ redTime, yellowTime+4
.equ debugFontWhite, redTime+4
.equ debugFontYellow, debugFontWhite+4
.equ debugFontRed, debugFontYellow+4
.thumb
@change the font color
ldr	r0,=#0x203FFE0
ldr	r0,[r0]
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

ldr	r1,=#0x203FFE0
ldr	r0,[r1]
cmp	r0,#0
beq	end
sub	r0,#1
str	r0,[r1]
end:
bx	lr
.align
.ltorg
yellowTime:
@WORD yellowTime
@WORD redTime
@POIN debugFontWhite
@POIN debugFontYellow
@POIN debugFontRed
