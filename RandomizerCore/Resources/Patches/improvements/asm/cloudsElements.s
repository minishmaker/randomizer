.thumb
@check if we are in the cloud tops map
ldr	r0,=#0x2000083
ldrb	r0,[r0]
cmp	r0,#0x0E
bne	draw

@check if we are at the lower cloud tops
ldr	r0,=#0x2032EE0
ldrb	r1,[r0]
cmp	r1,#8
bne	draw
ldrb	r0,[r0,#1]
cmp	r0,#0
beq	draw

@check if we are drawing an element
ldrb	r0,[r4,#3]
cmp	r0,#0x60
beq	element
cmp	r0,#0x61
beq	element
cmp	r0,#0x62
beq	element
cmp	r0,#0x63
beq	element
cmp	r0,#0x6D
beq	element
cmp	r0,#0x6E
beq	element
cmp	r0,#0x6F
beq	element
cmp	r0,#0x70
beq	element
b	draw

element:
mov	r0,#0
b	waselement

@draw the icon
draw:
ldrh	r0,[r4,#4]
waselement:
ldrh	r1,[r4,#6]
mov	r2,#0xFD
lsl	r2,#1
ldr	r3,=#0x80A6124
mov	lr,r3
ldrb	r3,[r4,#3]
.short	0xF800
