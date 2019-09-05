.equ custom, drawTime+4
.thumb
push	{r4-r6,lr}
push	{r0-r7}
ldr	r0,custom
cmp	r0,#0
beq	noCustom
mov	lr,r0
.short	0xF800
b	draw

noCustom:
ldr	r0,=#0x203FFF0
ldr	r0,[r0]

draw:
mov	r1,#0
mov	r2,#0
ldr	r3,drawTime
mov	lr,r3
ldr	r3,=#0x2035132
.short	0xF800

@set bg0 to update
ldr	r0,=#0x3000F5E
mov	r1,#1
strh	r1,[r0]

pop	{r0-r7}
ldr	r2,=#0x200AF00
ldrb	r1,[r2,#1]
mov	r0,#0x40
ldr	r6,=#0x801C4F1
bx	r6
.align
.ltorg
drawTime:
@POIN drawTime
@POIN custom
