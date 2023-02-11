.equ trapIconTable, customRNG+4
.thumb
push	{r4,r5}
mov	r4,r0
@check if witch shop
ldr	r0,=#0x3000BF0
ldrb	r0,[r0,#4]	
cmp	r0,#0x24
bne	notwitch
ldr	r4,=#0x3001160
notwitch:

@check if picking up animation
ldrb	r0,[r4,#0x09]
add	r0,#0x10
cmp	r0,#0x1B
beq	end

@get rng seed
ldr	r5,customRNG

@check if shop item
ldrb	r0,[r4,#0x09]
cmp	r0,#2
bne	notShop
mov	r0,#0x7E
ldrb	r0,[r4,r0]
lsl	r0,#24
add	r5,r0
notShop:

@get room and area
ldr	r0,=#0x3000BF0
ldrb	r1,[r0,#5]
ldrb	r0,[r0,#4]
add	r5,r0
lsl	r1,#8
add	r5,r1

@get the trap id
ldrb	r0,[r4,#0x0B]
lsl	r0,#16
add	r5,r0

@and the flag
mov	r0,#0x84
ldrh	r1,[r4,r0]
lsl	r1,#24
add	r5,r1
add	r0,#2
ldrh	r1,[r4,r0]
lsl	r1,#24
add	r5,r1

@take it down a bit
lsl	r5,#1
lsr	r5,#1

@get length of the list
ldr	r0,trapIconTable
mov	r1,#0
loop:
add	r1,#1
ldrb	r2,[r0]
add	r0,#1
cmp	r2,#0
bne	loop
sub	r1,#1

@and get the icon
mov	r0,r5
swi	#6
ldr	r0,trapIconTable
ldrb	r0,[r0,r1]

end:
pop	{r4,r5}
bx	lr
.align
.ltorg
customRNG:
@WORD customRNG
@POIN trapIconTable
