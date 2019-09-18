.equ timer, drawNumber+4
.equ target, timer+4
.equ counter, target+4
.thumb
and	r0,r1
mov	r6,r2
push	{r0-r7}
@check if we have a counter routine
ldr	r0,counter
cmp	r0,#0
beq	end

@check if we have a timer
ldr	r7,=#0x2035132
ldr	r0,timer
cmp	r0,#0
beq	notimer
@move the counter up
sub	r7,#0x40
notimer:

@load the numbers
ldr	r0,=#0x85C3150
ldr	r1,=#0x600C9E0
ldr	r2,=#0x600CB40
debug:
ldr	r3,[r0]
str	r3,[r1]
add	r0,#4
add	r1,#4
cmp	r1,r2
bne	debug

@clean the tile background
mov	r0,#0
mov	r1,#0
mov	r2,r7
loop1:
strh	r1,[r2]
add	r0,#1
add	r2,#2
cmp	r0,#7
bne	loop1

@draw target
ldr	r3,drawNumber
mov	lr,r3
mov	r3,r7
add	r3,#8
ldr	r0,target
mov	r1,#0
mov	r2,#0
.short	0xF800

@draw slash
mov	r0,#0x2F
strb	r0,[r7,#6]

@draw counter
ldr	r0,counter
mov	lr,r0
.short	0xF800
mov	r1,#0
mov	r2,#0
ldr	r3,drawNumber
mov	lr,r3
mov	r3,r7
add	r3,#4
.short	0xF800

@offset the tile ids
mov	r0,#0
mov	r2,r7
loop2:
ldrh	r1,[r2]
add	r1,#0x20
strh	r1,[r2]
add	r0,#1
add	r2,#2
cmp	r0,#7
bne	loop2

@set bg0 to update
ldr	r0,=#0x3000F5E
mov	r1,#1
strh	r1,[r0]

end:
pop	{r0-r7}
cmp	r0,#0
beq	return2
return1:
ldr	r3,=#0x801C4F9
bx	r3
return2:
ldr	r3,=#0x801C535
bx	r3
.align
.ltorg
drawNumber:
@POIN drawNumber
@WORD timer
@WORD target
@POIN counter
