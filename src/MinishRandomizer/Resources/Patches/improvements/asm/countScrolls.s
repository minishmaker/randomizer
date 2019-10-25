.thumb
@count total
mov	r5,#0
mov	r4,#0x48
countscroll:
mov	r0,r4
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	dontadd
add	r5,#1
dontadd:
add	r4,#1
cmp	r4,#0x4F
bls	countscroll

mov	r4,#0x73
countextra:
mov	r0,r4
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	dontaddextra
add	r5,#1
dontaddextra:
add	r4,#1
cmp	r4,#0x75
bls	countextra

@skip 1 tile if 10 or more
cmp	r5,#10
blo	np
add	r5,#1
np:

@load 10+ icon
ldr	r0,graphics
ldr	r1,=#0x6010160
ldr	r2,=#0x60101C0
loop:
ldr	r3,[r0]
str	r3,[r1]
add	r0,#4
add	r1,#4
cmp	r1,r2
blo	loop

ldr	r3,=#0x80A4E81
bx	r3
.align
.ltorg
graphics:
